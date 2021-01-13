using Microsoft.AspNetCore.Hosting;
using PractCoreCF.DAL;
using PractCoreCF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PractCoreCF.BAL
{
    public class UserRepository : IUserRepository
    {
        ApplicationDBContext dBContext;
        IHostingEnvironment environment;
        public UserRepository(ApplicationDBContext context, IHostingEnvironment _environment)
        {
            this.dBContext = context;
            this.environment = _environment;
        }
        public UserMaster AddUpdateUserMaster(UserMaster userMaster)
        {
            try
            {
                userMaster.StateMaster = null;
                userMaster.CountryMaster = null;
                var l_User = dBContext.UserMaster.Where(x => x.Email == userMaster.Email).FirstOrDefault();

                if (l_User != null)
                {
                    l_User.Address = userMaster.Address;
                    l_User.ContactNumber = userMaster.ContactNumber;
                    l_User.FirstName = userMaster.FirstName;
                    l_User.Email = userMaster.Email;
                    l_User.Gender = userMaster.Gender;
                    l_User.Hobbies = userMaster.Hobbies;
                    l_User.LastName = userMaster.LastName;
                    l_User.Password = userMaster.Password;
                    l_User.PostCode = userMaster.PostCode;
                    l_User.UpdateDate = DateTime.Now;
                    l_User.Gender = userMaster.Gender;
                    l_User.CountryId = userMaster.CountryId;
                    l_User.StateId = userMaster.StateId;
                    dBContext.Entry(l_User).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    dBContext.SaveChanges();
                    return l_User;
                }
                else
                {
                    userMaster.CreateDate = DateTime.Now;
                    userMaster.UpdateDate = DateTime.Now;
                    dBContext.UserMaster.Add(userMaster);
                }
                dBContext.SaveChanges();
                return userMaster;
            }
            catch (Exception ex)
            {
                ex.SetLog("AddUpdateUserMaster() UserRepository : " + ex.Message, environment);
                return null;
            }
        }

        public UserMaster GetLogin(string email, string password)
        {
            try
            {
                var l_User = dBContext.UserMaster.Where(x => x.Email == email && x.Password == password).FirstOrDefault();
                return l_User;
            }
            catch (Exception ex)
            {
                ex.SetLog("AddUpdateUserMaster() UserRepository : " + ex.Message, environment);
                return null;
            }
        }

        /// <summary>
        /// Without transaction management
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        //public bool DeleteUser(int UserId)
        //{
        //    try
        //    {
        //        var l_user = dBContext.UserMaster.Where(x => x.UserId == UserId).FirstOrDefault();
        //        if (l_user != null)
        //        {
        //            //Get all images uploaded by user
        //            var l_UserImages = dBContext.UserImages.Where(x => x.UserId == UserId).ToList();

        //            //Delete all images if exists uploaded by user
        //            foreach (var i_image in l_UserImages)
        //            {
        //                try
        //                {
        //                    dBContext.UserImages.Remove(i_image);
        //                    dBContext.SaveChanges();
        //                }
        //                catch (Exception ex)
        //                {
        //                    ex.SetLog("DeleteUser()==>UserImages.Remove  UserRepository() : " + ex.Message, environment);
        //                    continue;
        //                }
        //            }

        //            //get token of user
        //            var l_token = dBContext.TokenMaster.Where(x => x.UserId == UserId).FirstOrDefault();

        //            //Delete token 
        //            if (l_token != null)
        //            {
        //                try
        //                {
        //                    dBContext.TokenMaster.Remove(l_token);
        //                    dBContext.SaveChanges();
        //                }
        //                catch (Exception ex)
        //                {
        //                    ex.SetLog("DeleteUser()==> TokenMaster.Remove  UserRepository() : " + ex.Message, environment);
        //                }
        //            }

        //            try
        //            {
        //                dBContext.UserMaster.Remove(l_user);
        //                dBContext.SaveChanges();
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                ex.SetLog("DeleteUser() UserRepository() : " + ex.Message, environment);
        //                return false;
        //            }
        //        }
        //        else
        //            return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.SetLog("DeleteUser() UserRepository()" + ex.Message, environment);
        //        throw;
        //    }
        //}


        
        
        ///<summary>
        ///With transaction management
        /// </summary>
        public bool DeleteUser(int UserId)
        {
            using (var contextTransaction = dBContext.Database.BeginTransaction())
            {
                try
                {
                    var l_user = dBContext.UserMaster.Where(x => x.UserId == UserId).FirstOrDefault();
                    if (l_user != null)
                    {
                        //First get all images data from database uploaded by user
                        var l_userImages = dBContext.UserImages.Where(x => x.UserId == UserId).ToList();
                        foreach (var i_image in l_userImages)
                        {
                            dBContext.UserImages.Remove(i_image);
                            dBContext.SaveChanges();
                        }

                        //Get token data of user
                        var l_token = dBContext.TokenMaster.Where(x=>x.UserId==UserId).FirstOrDefault();
                        if (l_token != null)
                        {
                            dBContext.TokenMaster.Remove(l_token);
                            dBContext.SaveChanges();
                        }

                        //Finally deete user
                        //dBContext.UserMaster.Where(x=>x.UserId==UserId).FirstOrDefault();
                        dBContext.UserMaster.Remove(l_user);
                        dBContext.SaveChanges();
                    }
                    contextTransaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    contextTransaction.Rollback();
                    return false;
                }
            }
        }

        public List<CountryMaster> GetAllCountries()
        {
            try
            {
                var l_countries = dBContext.CountryMaster.ToList();
                return l_countries;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<UserMaster> GetAllUsers()
        {
            try
            {
                var l_Users = dBContext.UserMaster.ToList();
                return l_Users;
            }
            catch (Exception ex)
            {
                ex.SetLog("GetAllUsers() UserRepository() : " + ex.Message, environment);
                throw;
            }
        }

        public CountryMaster GetCountryById(int CountryId)
        {
            try
            {
                var l_country = dBContext.CountryMaster.Where(x => x.CountryId == CountryId).FirstOrDefault();
                return l_country;
            }
            catch (Exception ex)
            {
                ex.SetLog("GetCountryById() UserRepository " + ex.Message, environment);
                throw;
            }
        }

        public StateMaster GetStateById(int StateId)
        {
            try
            {
                var l_state = dBContext.StateMaster.Where(x => x.StateId == StateId).FirstOrDefault();
                return l_state;
            }
            catch (Exception ex)
            {
                ex.SetLog("GetStateById() UserRepository : " + ex.Message, environment);
                throw;
            }
        }

        public List<StateMaster> GetStateListByCountryId(int CountryId)
        {
            try
            {
                var l_stateList = dBContext.StateMaster.Where(x => x.CountryId == CountryId).ToList();
                return l_stateList;
            }
            catch (Exception ex)
            {
                ex.SetLog("GetStateListByCountryId() GetStateListByCountryId() : " + ex.Message, environment);
                throw;
            }
        }

        public UserMaster GetUserByUserId(int UserId)
        {
            try
            {
                var l_User = dBContext.UserMaster.Where(x => x.UserId == UserId).FirstOrDefault();
                //try
                //{
                //    l_User.Password = CommonFunction.Decrypt(l_User.Password);
                //}
                //catch (Exception ex)
                //{ 
                
                //}
                return l_User;
            }
            catch (Exception ex)
            {
                ex.SetLog("GetUserById() UserRepository() : " + ex.Message, environment);
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                this.dBContext.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        public bool AddUpdateTokenMaster(TokenMaster token)
        {
            try
            {
                var l_token = dBContext.TokenMaster.Where(x => x.UserId == token.UserId).FirstOrDefault();
                //Add token
                if (l_token == null)
                {
                    try
                    {
                        token.CreateDate = DateTime.Now;
                        token.UpdateDate = DateTime.Now;
                        dBContext.TokenMaster.Add(token);
                        dBContext.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        ex.SetLog("AddUpdateTokenMaster() AddFailed UserRepository()" + ex.Message, environment);
                        return false;
                    }
                }
                //Update token
                else
                {
                    try
                    {
                        l_token.Token = token.Token;
                        l_token.UpdateDate = DateTime.Now;
                        dBContext.Entry(l_token).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        dBContext.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        ex.SetLog("AddUpdateTokenMaster()==UpdateFailed UserRepo : " + ex.Message, environment);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetTokenByUserId(int UserId)
        {
            try
            {
                if (UserId > 0)
                {
                    //Here finvd token
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool GetAuthentication(string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var l_TokenMaster = dBContext.TokenMaster.Where(x => x.Token == token).FirstOrDefault();
                    if (l_TokenMaster != null)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int GetUserIdFromToken(string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var l_TokenMaster = dBContext.TokenMaster.Where(x => x.Token == token).FirstOrDefault();
                    if (l_TokenMaster != null)
                    {
                        return l_TokenMaster.UserId;
                    }
                    return 0;
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public UserMaster getUserInformationByToken(string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var l_userId = GetUserIdFromToken(token);
                    if (l_userId > 0)
                    {
                        return GetUserByUserId(l_userId);
                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteUserImage(int imageId)
        {
            try
            {
                var l_image = dBContext.UserImages.Where(x => x.UserImageId == imageId).FirstOrDefault();
                if (l_image != null)
                {
                    dBContext.UserImages.Remove(l_image);
                    dBContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<UserImages> GetUserImagesByUserId(int UserId)
        {
            try
            {
                return dBContext.UserImages.Where(x => x.UserId == UserId).ToList();
            }
            catch (Exception ex)
            {
                return new List<UserImages>();
            }
        }
        public bool DeleteAllImagesByUserId(int UserId)
        {
            try
            {
                var l_userImages = dBContext.UserImages.Where(x => x.UserId == UserId).ToList();
                foreach (var l_image in l_userImages)
                {
                    try
                    {
                        dBContext.UserImages.Remove(l_image);
                        dBContext.SaveChanges();
                    }
                    catch (Exception ex)
                    { continue; }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveUserImage(UserImages image)
        {
            try
            {
                dBContext.UserImages.Add(image);
                dBContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }

        public bool SaveMultipleUserImages(List<UserImages> userImages)
        {
            try
            {
                foreach (var l_image in userImages)
                {
                    try
                    {
                        dBContext.Add(l_image);
                        dBContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
