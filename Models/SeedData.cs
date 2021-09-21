using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TBay.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TBay.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            IdentityResult roleResult;
            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            roleCheck = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("User"));
            }


            AppUser user = await UserManager.FindByEmailAsync("nikolov.tea@gmail.com");
            if (user == null)
            {
                var User = new AppUser();
                User.Email = "nikolov.tea@gmail.com";
                User.UserName = "Tea";
                User.Role = "Admin";
                string userPWD = "Admin123@";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin      
                if (chkUser.Succeeded)
                {
                    var result1 = await UserManager.AddToRoleAsync(User, "Admin");
                }
            }
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TBayContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<TBayContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
                // Look for any stores.
                //if (context.Store.Any() || context.Item.Any() || context.Designer.Any() || context.User.Any())
                //{
                //    return;   // DB has been seeded
                //}
                List<Designer> listDesigners = new List<Designer>
                {
                      new Designer
                   {
                       Picture = "1.jpg",
                       FirstName = "Alexander",
                       LastName = "McQueen",
                       DateofBirth = DateTime.Parse("1969-3-17"),
                       Biography = " Lee Alexander McQueen (17 March 1969 – 11 February 2010) was an English fashion designer and couturier. He founded his own Alexander McQueen label in 1992, and was chief designer at Givenchy from 1996 to 2001.",
                       Items = ""
                   },
                    new Designer
                    {
                        Picture = "2.jpg",
                        FirstName = "Pierre",
                        LastName = "Balmain",
                        DateofBirth = DateTime.Parse("1914-5-17"),
                        Biography = "Pierre Alexandre Claudius Balmain (18 May 1914 – 29 June 1982) was a French fashion designer and founder of leading post-war fashion house Balmain. Known for sophistication and elegance, he described the art of dressmaking as the architecture of movement",
                        Items = ""
                    },
                       new Designer
                       {
                           Picture = "3.jpg",
                           FirstName = "Guccio",
                           LastName = "Gucci",
                           DateofBirth = DateTime.Parse("1881-3-26"),
                           Biography = "In 1921, he founded the House of Gucci in Florence as a small family-owned leather shop. He began selling leather bags to horsemen in the 1920s. In 1938, Gucci expanded his business to a second location in Rome, at the insistence of his son Aldo. His one-man business eventually turned into a family business, when his sons joined the company. In 1951, Gucci opened their store in Milan. He wanted to keep the business small and while he was alive, the company remained only in Italy. Two weeks before Guccio Gucci's death, the New York Gucci boutique was opened by his sons Aldo, Rodolfo, and Vasco.",
                           Items = ""
                       },
                       new Designer
                       {
                           Picture = "4.jpg",
                           FirstName = "Tom",
                           LastName = "Ford",
                           DateofBirth = DateTime.Parse("1961-8-27"),
                           Biography = "Thomas Carlyle Ford (born August 27, 1961) is an American fashion designer and filmmaker. He launched his eponymous luxury brand in 2006, having previously served as the creative director at Gucci and Yves Saint Laurent. Ford wrote and directed the Academy Award–nominated films A Single Man (2009) and Nocturnal Animals (2016). He currently serves as the Chairman of the Board of the Council of Fashion Designers of America.",
                           Items = ""
                       },
                      new Designer
                      {
                          Picture = "5.jpg",
                          FirstName = "Louis",
                          LastName = "Vuitton",
                          DateofBirth = DateTime.Parse("1821-8-4"),
                          Biography = "Louis Vuitton (4 August 1821 – 27 February 1892) was a French fashion designer and businessman. He was the founder of the Louis Vuitton brand of leather goods now owned by LVMH. Prior to this, he had been appointed as trunk-maker to Empress Eugénie de Montijo, wife of Napoleon III.",
                          Items = ""
                      }
                };
                for (int i = 0; i < listDesigners.Count; i++)
                {
                    var designer = from a in context.Designer
                                   where a.Picture == listDesigners[i].Picture
                                   select a;
                    if (designer == null || designer.Count()==0)
                    {
                        context.Add(listDesigners[i]);
                    }
                };

                context.SaveChanges();
                List<Item> listItems = new List<Item>
                {
                    new Item
                    {
                        Picture = "alexandar mcqueen1.jpeg",
                        Designerid = context.Designer.FirstOrDefault(d => d.FirstName == "Alexander" && d.LastName == "McQueen")==null? 0:context.Designer.FirstOrDefault(d => d.FirstName == "Alexander" && d.LastName == "McQueen").DesignerID,
                        Name = "Alexander McQueen Oversize Sneaker",
                        Category = "Shoes",
                        Price = 200,
                        Size = "38"
                    },
                     new Item
                     {
                         Picture = "alexandar mcqueen2.jpeg",
                         Designerid = context.Designer.FirstOrDefault(d => d.FirstName == "Alexander" && d.LastName == "McQueen").DesignerID,
                         Name = "Alexander McQueen White Dress",
                         Category = "Dresses",
                         Price = 120,
                         Size = "S"
                     },
                     new Item
                     {

                         Picture = "balmain.jpg",
                         Designerid = context.Designer.FirstOrDefault(d => d.FirstName == "Pierre" && d.LastName == "Balmain").DesignerID,
                         Name = "Balmain Cowgirl Jeans",
                         Category = "Jeans",
                         Price = 89,
                         Size = "M"
                     },
                       new Item
                       {
                           Picture = "balmain2.jpg",
                           Designerid = context.Designer.FirstOrDefault(d => d.FirstName == "Pierre" && d.LastName == "Balmain").DesignerID,
                           Name = "Balmain Sunglasses",
                           Category = "Sunglasses",
                           Price = 70,
                           Size = "M"
                       },
                        new Item
                        {
                            Picture = "guci1.jpg",
                            Designerid = context.Designer.FirstOrDefault(d => d.FirstName == "Guccio" && d.LastName == "Gucci").DesignerID,
                            Name = "Gucci Shoes",
                            Category = "Shoes",
                            Price = 300,
                            Size = "37"
                        },
                        new Item
                        {
                            Picture = "guci2.jpg",
                            Designerid = context.Designer.FirstOrDefault(d => d.FirstName == "Guccio" && d.LastName == "Gucci").DesignerID,
                            Name = "Gucci jeans",
                            Category = "Jeans",
                            Price = 200,
                            Size = "S"
                        },
                        new Item
                        {
                            Picture = "tomford1.jpg",
                            Designerid = context.Designer.FirstOrDefault(d => d.FirstName == "Tom" && d.LastName == "Ford").DesignerID,
                            Name = "Sunny",
                            Category = "Sunglasses",
                            Price = 190,
                            Size = "S"
                        },
                        new Item
                        {
                            Picture = "viton1.jpg",
                            Designerid = context.Designer.FirstOrDefault(d => d.FirstName == "Louis" && d.LastName == "Vuitton").DesignerID,
                            Name = "Trendy bags",
                            Category = "Bags",
                            Price = 90,
                            Size = "S"
                        }
                };
                for (int i = 0; i < listItems.Count; i++)
                {
                    var item = from a in context.Item
                                   where a.Picture == listItems[i].Picture
                                   select a;
                    if (item == null || item.Count()==0)
                    {
                        context.Add(listItems[i]);
                    }
                };
                context.SaveChanges();


                List<Store> listStores = new List<Store>
                {
                    new Store
                    {
                        Picture = "stok.jpg",
                        Name = "StockX",
                        ItemId = context.Item.FirstOrDefault(d => d.Name == "Alexander McQueen Oversize Sneaker").ItemsID,
                        Rating = "8.1/10",
                        Link = "https://stockx.com/"
                    },
                    new Store
                    {
                        Picture = "harrods.jpg",
                        Name = "Harrods",
                        ItemId = context.Item.FirstOrDefault(d => d.Name == "Alexander McQueen White Dress").ItemsID,
                        Rating = "6.5/10",
                        Link = "https://www.harrods.com/en-mk/"
                    },
                    new Store
                    {
                        Picture = "6pm.png",
                        Name = "6pm",
                        ItemId = context.Item.FirstOrDefault(d => d.Name == "Balmain Cowgirl Jeans").ItemsID,
                        Rating = "9/10",
                        Link = "https://www.6pm.com/"
                    },
                    new Store
                    {
                        Picture = "mad.png",
                        Name = "Madewell",
                        ItemId = context.Item.FirstOrDefault(d => d.Name == "Balmain Sunglasses").ItemsID,
                        Rating = "9.8/10",
                        Link = "https://www.madewell.com/"
                    },
                    new Store
                    {
                        Picture = "rebag.png",
                        Name = "Rebag",
                        ItemId = context.Item.FirstOrDefault(d => d.Name == "Trendy bags").ItemsID,
                        Rating = "7/10",
                        Link = "https://www.rebag.com/"
                    },
                 new Store
                 {
                     Picture = "rr.jpg",
                     Name = "The RealReal",
                     ItemId = context.Item.FirstOrDefault(d => d.Name == "Sunny").ItemsID,
                     Rating = "8/10",
                     Link = "https://www.therealreal.com/"
                 }
                };
                for (int i = 0; i < listStores.Count; i++)
                {
                    var store = from a in context.Store
                               where a.Picture == listStores[i].Picture
                               select a;
                    if (store == null || store.Count() == 0)
                    {
                        context.Add(listStores[i]);
                    }
                };
                context.SaveChanges();



            }
        }
    }
}
