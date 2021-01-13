using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PractCoreCF.BAL;
using PractCoreCF.DAL;
using PractCoreCF.Models;
using PractCoreCF.ModelView;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PractCoreCF.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration _config;
        //private ApplicationDBContext _context;
        //DAL.ISDAL ISDAL;

        IUserRepository _userRepository;
        IHostingEnvironment environment;
        public AccountController(IConfiguration config, ApplicationDBContext dBContext, IUserRepository userRepository, IHostingEnvironment _environment)
        {
            //ISDAL = new DAL.ISDAL(dBContext);
            _config = config;
            //_context = dBContext;

            this.environment = _environment;

            this._userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("account/login")]
        public IActionResult Login()
        {
            if (IsAuthenticated())
                return RedirectToAction("Index");

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("account/login")]
        public IActionResult Login(LoginModel user)
        {
            //var l_user =
            if (ModelState.IsValid)
            {
                var l_Res = _userRepository.GetLogin(user.Email, user.Password = CommonFunction.Encrypt(user.Password));

                if (l_Res != null)
                {
                    var l_token = GenerateJSONWebToken(l_Res);
                    //If exists then remove old cookie 
                    Response.Cookies.Delete("AuthToken");
                    Response.Cookies.Delete("UserId");
                    Response.Cookies.Delete("UserName");

                    //Add cookie in browser 
                    Response.Cookies.Append("AuthToken", l_token);
                    Response.Cookies.Append("UserId", l_Res.UserId.ToString());
                    Response.Cookies.Append("UserName", l_Res.FirstName +" "+l_Res.LastName);

                    var l_saveToken = _userRepository.AddUpdateTokenMaster(new TokenMaster()
                    {
                        Token = l_token,
                        UserId = l_Res.UserId
                    });

                    return RedirectToAction("Index");
                }

                ViewBag.Countries = _userRepository.GetAllCountries();

                //Repo call here

            }
            return View(user);
        }

        //[HttpGet]
        [Route("account/UserInfo/{UserId?}")]
        public IActionResult UserInfo(int? UserId = null)
        {
            if (IsAuthenticated())
            {
                if (UserId != null && UserId != 0)
                {
                    var l_user = _userRepository.GetUserByUserId(UserId.GetValueOrDefault());
                    if (l_user != null)
                    {
                        l_user.Password = CommonFunction.Decrypt(l_user.Password);
                        return View(Mapper.UserMapper.MapSingleUserToUserMasterView(l_user));
                    }
                    return new NotFoundResult();
                }
                else
                {
                    var l_token = (string)Request.Cookies["AuthToken"];
                    if (!string.IsNullOrEmpty(l_token))
                    {
                        var l_user = _userRepository.getUserInformationByToken(l_token);
                        l_user.Password = CommonFunction.Decrypt(l_user.Password);
                        if (l_user != null)
                        {
                            return View(Mapper.UserMapper.MapSingleUserToUserMasterView(l_user));
                        }
                        return new NotFoundResult();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(new UserMasterView());
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("account/UserInfo")]
        public IActionResult UserInfo(UserMasterView user)
        {

           
            if (ModelState.IsValid)
            {
                if (user.CountryId > 0)
                {
                    var l_country = _userRepository.GetCountryById(user.CountryId);
                    if (l_country != null)
                    {
                        user.CountryMaster = Mapper.UserMapper.MapSingleCountryToCountryMasterView(l_country);
                    }
                    if (user.StateId > 0)
                    {
                        var l_state = _userRepository.GetStateById(user.StateId);
                        if (l_state != null)
                        {
                            user.StateMaster = Mapper.UserMapper.MapSingleStateToStateMasterView(l_state);
                        }
                    }
                }
                user.Password=CommonFunction.Encrypt(user.Password);
                var l_Res = _userRepository.AddUpdateUserMaster(Mapper.UserMapper.MapSingleUserToUserMaster(user));

                if (l_Res != null)
                {

                    if (Request.Cookies["UserId"] == l_Res.UserId.ToString())
                    {
                        Response.Cookies.Delete("UserName");
                        Response.Cookies.Append("UserName",l_Res.FirstName+" "+l_Res.LastName);
                    }
                    if (Request.Form.Files.Count > 0)
                    {
                        List<string> files = new List<string>();
                        foreach (var formFile in Request.Form.Files)
                        {
                            if (formFile.Length > 0)
                            {
                                //get path
                                var l_projectRootPath = environment.WebRootPath;
                                //var l_projectRootPath = environment.ContentRootPath;
                                l_projectRootPath = l_projectRootPath + "\\Uploads\\" + l_Res.UserId.ToString() + "\\";

                                try
                                {
                                    if (!System.IO.Directory.Exists(l_projectRootPath))
                                    {
                                        System.IO.Directory.CreateDirectory(l_projectRootPath);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ex.SetLog("Error while create directory : UserInfo() AccountController() : " + ex.Message, environment);
                                }
                                //var filePath = l_projectRootPath + formFile.FileName;
                                var l_extension = Path.GetExtension(formFile.FileName);
                                var l_FileName = Path.GetFileNameWithoutExtension(formFile.FileName);
                                l_FileName = l_FileName + Guid.NewGuid().ToString()+ l_extension;
                                var filePath = l_projectRootPath + l_FileName;
                                try
                                {
                                    using (var stream = System.IO.File.Create(filePath))
                                    {
                                        //Save file
                                        formFile.CopyTo(stream);
                                        //var l_filename = "\\Uploads\\" + l_Res.UserId.ToString() + "\\" + formFile.FileName;
                                        var l_filename = "\\Uploads\\" + l_Res.UserId.ToString() + "\\" + l_FileName;
                                        files.Add(l_filename);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ex.SetLog("Error while save images to folder, UserInfo() AccountController() : " + ex.Message, environment);
                                }
                            }

                        }

                        //Get all old images 
                        var l_userOldImages = _userRepository.GetUserImagesByUserId(l_Res.UserId);

                        //Delete all Old images from folder, so no garbage collection is there which takes space
                        var l_FolderRootPath = environment.WebRootPath + "\\";
                        foreach (var l_OldImage in l_userOldImages)
                        {
                            try
                            {
                                if (System.IO.File.Exists(l_FolderRootPath + l_OldImage.FileName))
                                {
                                    System.IO.File.Delete(l_FolderRootPath + l_OldImage.FileName);
                                }
                            }
                            catch (Exception ex)
                            {
                                ex.SetLog("Error while delete Image from folder , Userinfo(), AccountController() : " + ex.Message, environment);
                                continue;
                            }
                        }

                        //Delete All Old images from database and save latest uploaded images
                        var l_res = _userRepository.DeleteAllImagesByUserId(l_Res.UserId);

                        List<UserImages> l_Images = new List<UserImages>();
                        foreach (var l_strFile in files)
                        {
                            l_Images.Add(new UserImages()
                            {
                                FileName = l_strFile,
                                UserId = l_Res.UserId
                            });

                            ////We can save single images from here
                            //_userRepository.SaveUserImage(new UserImages()
                            //{
                            //    FileName = l_strFile,
                            //    UserId = user.UserId
                            //});
                        }
                        //Save multiple images at time
                        var l_respImages = _userRepository.SaveMultipleUserImages(l_Images);
                    }

                    #region Token generation and save to database, add cookie in browser
                    //var l_token = GenerateJSONWebToken(l_Res);

                    ////save token
                    //_userRepository.AddUpdateTokenMaster(new TokenMaster()
                    //{
                    //    CreateDate = DateTime.Now,
                    //    Token = l_token,
                    //    UpdateDate = DateTime.Now,
                    //    UserId = l_Res.UserId
                    //});

                    ////Add cookie in browser, also clear old cookie
                    //Response.Cookies.Delete("AuthToken");
                    //Response.Cookies.Append("AuthToken", l_token);
                    #endregion


                    ////We can also manage auto login after user successfully sign up
                    //return RedirectToAction("Login", new LoginModel()
                    //{
                    //    Email = l_Res.Email,
                    //    Password = l_Res.Password
                    //});

                    return RedirectToAction("Login");
                }

                ViewBag.Countries = _userRepository.GetAllCountries();
                //Repo call here
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Route("account/Index")]
        public IActionResult Index()
        {
            if (IsAuthenticated())
            {
                var l_UserData = _userRepository.GetAllUsers();
                return View(Mapper.UserMapper.MapMultipleUserToUserMasterView(l_UserData));
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("account/logout")]
        public IActionResult Logout()
        {
            try
            {
                var l_cookie = (string)Request.Cookies["AuthToken"];

                var l_userId = _userRepository.GetUserIdFromToken(l_cookie);

                if (l_userId > 0)
                {
                    var l_res = _userRepository.AddUpdateTokenMaster(new TokenMaster()
                    {
                        Token = "Logged Out",
                        UserId = l_userId
                    });

                    if (l_res)
                    {
                        Response.Cookies.Delete("AuthToken");
                        Response.Cookies.Delete("UserId");
                        Response.Cookies.Delete("UserName");
                        return RedirectToAction("Login");
                    }
                }

                //previous url
                var l_prevPage = Request.Headers["Referer"].ToString();

                if (!string.IsNullOrEmpty(l_prevPage))
                    return Redirect(l_prevPage);

                else
                    return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ex.SetLog("Error while logout user, Logout() AccountController : " + ex.Message, environment);
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        [Route("account/Delete/{UserId}")]
        public IActionResult Delete(int UserId)
        {

            //delete user images folder to free up space
            var l_UerImageFolderPath = environment.WebRootPath;
            l_UerImageFolderPath = l_UerImageFolderPath + "\\Uploads\\" + UserId.ToString();
            try
            {
                if (System.IO.Directory.Exists(l_UerImageFolderPath))
                {
                    //We will put recursive true, If there is any folder inside it will also be deleted
                    System.IO.Directory.Delete(l_UerImageFolderPath, true);
                }
                //Here we will delete user data after successfully deleted images folder

                var l_DelResult = _userRepository.DeleteUser(UserId);

                if (l_DelResult)
                { 
                    return Ok("Success");
                }
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), "Something went wrong while deleting process....!!!!");
            }
            catch (Exception ex)
            {
                ex.SetLog("Error while Delete UserImages folder Delete(UserId) AccountController : " + ex.Message, environment);
                return StatusCode(500,ex);
            }
        }

        [HttpGet]
        [Route("account/galary/{UserId}")]
        public IActionResult Galary(int UserId)
        {
            if (IsAuthenticated())
            {
                var l_UserImagesData = _userRepository.GetUserImagesByUserId(UserId);
                return View(Mapper.UserMapper.MapMultipleImageToImageMasterView(l_UserImagesData));
            }
            return RedirectToAction("Login");

        }
    
        #region Methods for authentication
        private string GenerateJSONWebToken(UserMaster userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserId+","+userInfo.FirstName +" "+userInfo.LastName),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim("DateOfJoining", userInfo.CreateDate.ToString("yyyy-MM-dd")),
                new Claim("UserId", userInfo.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsAuthenticated()
        {
            var l_cookie = (string)Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(l_cookie))
            {
                return _userRepository.GetAuthentication(l_cookie);
            }
            return false;
        }

        #endregion
    }
}
