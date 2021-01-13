using PractCoreCF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PractCoreCF.DAL
{
    public class ISDAL
    {
        ApplicationDBContext dBContext;
        public ISDAL(ApplicationDBContext context)
        {
            this.dBContext = context;
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
                return null;
            }
        }

        public UserMaster AddUserUpdate(UserMaster userMaster)
        {
            try
            {
                var l_User = dBContext.UserMaster.Where(x => x.Email == userMaster.Email).FirstOrDefault();

                if (l_User != null)
                {
                    l_User.Address = userMaster.Address;
                    l_User.ContactNumber = userMaster.ContactNumber;
                    l_User.FirstName= userMaster.FirstName;
                    l_User.Email = userMaster.Email;
                    l_User.Gender = userMaster.Gender;
                    l_User.Hobbies = userMaster.Hobbies;
                    l_User.LastName = userMaster.LastName;
                    l_User.Password = userMaster.Password;
                    l_User.PostCode = userMaster.PostCode;
                    l_User.UpdateDate = DateTime.Now;
                    dBContext.Entry(l_User).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    return l_User;
                }
                else
                {
                    userMaster.CreateDate = DateTime.Now;
                    userMaster.UpdateDate = DateTime.Now;
                    dBContext.UserMaster.Add(userMaster);
                }
                
                dBContext.SaveChanges();
                return l_User;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<UserMaster> GetAllUsers()
        {
            try
            {
                var l_users= dBContext.UserMaster.ToList();
                
                return l_users;
            }
            catch (Exception ex)
            {
                return new List<UserMaster>();
            }
        }

        public UserMaster GetUserById(int f_UserID) {
            try {
                var l_user = dBContext.UserMaster.Where(x => x.UserId == f_UserID).FirstOrDefault();
                return l_user;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public bool DeleteUserByUserId(int f_UserId)
        {
            try
            {
                var l_usr = dBContext.UserMaster.Where(x => x.UserId == f_UserId).FirstOrDefault();
                if (l_usr != null)
                {
                    var l_images = dBContext.UserImages.Where(x => x.UserId == f_UserId).ToList();
                    foreach (var image in l_images)
                    {
                        try
                        {
                            dBContext.UserImages.Remove(image);
                            dBContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                    var l_token = dBContext.TokenMaster.Where(x => x.UserId == f_UserId).FirstOrDefault();
                    if (l_token != null)
                    {
                        try
                        {
                            dBContext.TokenMaster.Remove(l_token);
                            dBContext.SaveChanges();
                        }
                        catch (Exception ex)
                        { 
                        
                        }
                    }

                    //Finally delete record from user master table after delete all dependent records
                    try
                    {
                        dBContext.UserMaster.Remove(l_usr);
                        dBContext.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
