using CafeShopFPT.Models;
using log4net;
using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CafeShopFPT.DAO.AccountsDao
{
    public class AccountDao
    {
        private static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static AccountDao instance;

        public static AccountDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountDao();
                }
                return instance;
            }
            private set => instance = value;
        }

        private AccountDao()
        {
        }

        private AccountDTO _currentUser;
        public AccountDTO CurrrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
            }
        }

        public bool Authorization(string userName, string passWord)
        {

            try
            {

                Log.Info("Authorization start.");

                bool verified = false;
                Account getAccount = DataProvider.Ins.DB.Accounts.Include(x => x.TypeNavigation)
                    .Where(account => account.UserName.Equals(userName)).FirstOrDefault();

                if (getAccount != null)
                {
                    verified = BCrypt.Net.BCrypt.Verify(passWord, getAccount.PassWord);
                    if (verified)
                    {
                        CurrrentUser = new AccountDTO
                        {
                            AccountId = getAccount.AccountId,
                            Avatar = getAccount.Avatar,
                            DisplayName = getAccount.DisplayName,
                            Email = getAccount.UserName,
                            PassWord = getAccount.PassWord,
                            Phone = getAccount.Phone,
                            Role = new RoleDao.RoleDto { Id = getAccount.TypeNavigation.Id, Name = getAccount.TypeNavigation.Name },

                            Type = getAccount.Type,

                        };
                    }
                }
                return verified;

            }
            catch (Exception ex)
            {

                Log.Error(ex);
                return false;

            }
            finally
            {

                Log.Info("Authorization end.");
            }
        }


        public string GetAccountIdMax()
        {

            try
            {
                var maxId = DataProvider.Ins.DB.Accounts.Max(x => x.AccountId);
                if (string.IsNullOrEmpty(maxId))
                {
                    return (0).ToString().PadLeft(10, '0');
                }
                else
                {
                    return (Convert.ToInt32(maxId) + 1).ToString().PadLeft(10, '0');
                }
            }
            catch (Exception)
            {

                return null;
            }


        }

        public List<AccountDTO> LoadAllAccount()
        {
            try
            {
                var result = (from account in DataProvider.Ins.DB.Accounts
                              select new AccountDTO
                              {
                                  AccountId = account.AccountId,
                                  Avatar = account.Avatar,
                                  DisplayName = account.DisplayName,
                                  PassWord = account.PassWord,
                                  Type = account.Type,
                                  Email = account.UserName,
                                  Phone = account.Phone,
                                  Role = new RoleDao.RoleDto
                                  {
                                      Id = account.TypeNavigation.Id,
                                      Name = account.TypeNavigation.Name,
                                  },
                              }
                              );


                return result.ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsAccountExist(string username)
        {
            var result = DataProvider.Ins.DB.Accounts.Where(x => x.UserName.Equals(username)).FirstOrDefault() != null ? true : false;

            return result;

        }

        public bool UpdateAccount(AccountDTO account)
        {
            try
            {


                var updateAccount = DataProvider.Ins.DB.Accounts.Where(x => x.AccountId.Equals(account.AccountId)).FirstOrDefault();
                if (updateAccount != null)
                {
                    updateAccount.Avatar = System.IO.Path.GetFileName(account.Avatar);
                    updateAccount.PassWord = account.PassWord;
                    updateAccount.DisplayName = account.DisplayName;
                    updateAccount.Type = account.Type;
                    updateAccount.Phone = account.Phone;

                }

                DataProvider.Ins.DB.Accounts.Update(updateAccount);
                DataProvider.Ins.SaveChanges();

                return true;
            }
            catch (System.Exception)
            {

                return false;
                throw;
            }
        }

        public bool AddAccount(AccountDTO account)
        {
            try
            {

                var addAccount = new Account
                {
                    AccountId = account.AccountId,
                    Avatar = System.IO.Path.GetFileName(account.Avatar),
                    PassWord = account.PassWord,
                    DisplayName = account.DisplayName,
                    Type = account.Type,
                    UserName = account.Email,
                    Phone = account.Phone,
                };

                DataProvider.Ins.DB.Accounts.Add(addAccount);
                DataProvider.Ins.SaveChanges();

                return true;

            }
            catch (System.Exception)
            {

                return false;
                throw;
            }
        }

        public bool RemoveAccount(string accountId)
        {
            try
            {


                var removeAccount = DataProvider.Ins.DB.Accounts.Where(x => x.AccountId.Equals(accountId)).FirstOrDefault();
                if (removeAccount != null)
                {

                    DataProvider.Ins.DB.Accounts.Remove(removeAccount);
                    DataProvider.Ins.SaveChanges();

                    return true;
                }

                return false;
            }
            catch (System.Exception)
            {

                return false;
                throw;
            }
        }

    }
}
