﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dropthings.DataAccess;

using System.Transactions;

namespace Dropthings.Business.Facade
{
	partial class Facade
    {

        #region Methods
        public UserSetting GetUserSetting(Guid userGuid)
        {
            using (new TimedLog(userGuid.ToString(), "Activity: Get User Setting"))
            {
                var userSetting = this.userSettingRepository.GetUserSettingByUserGuid(userGuid);

                if (userSetting == default(UserSetting))
                {
                    // No setting saved before. Create default setting

                    userSetting = this.userSettingRepository.Insert(
                        newSetting =>
                        {
                            newSetting.UserId = userGuid;
                            newSetting.CreatedDate = DateTime.Now;
                            newSetting.CurrentPageId = this.pageRepository.GetPageIdByUserGuid(userGuid).First();
                        });
                }

                return userSetting;
            }
        }

        public bool TransferOwnership(Guid userOldGuid)
        {
            var success = false;

            using (TransactionScope ts = new TransactionScope())
            {
                List<Page> pages = this.pageRepository.GetPagesOfUser(userOldGuid);
                this.pageRepository.UpdateList(pages, (page) =>
                {
                    page.UserId = userOldGuid;
                }, null);
                
                var userSetting = GetUserSetting(userOldGuid);
                
                // Delete setting for the anonymous user and create new setting for the new user 
                this.userSettingRepository.Delete(userSetting);

                this.userSettingRepository.Insert((newSetting) =>
                {
                    newSetting.UserId = userOldGuid;
                    newSetting.CurrentPageId = userSetting.CurrentPageId;
                    newSetting.CreatedDate = DateTime.Now;
                });

                ts.Complete();
            }

            return success;
        }

        public bool UserExists(string userName)
        {
            return (this.userRepository.GetUserGuidFromUserName(userName) != default(Guid));
        }

        #endregion
    }
}
