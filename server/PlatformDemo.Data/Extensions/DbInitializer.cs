using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using PlatformDemo.Core.Extensions;
using PlatformDemo.Data.Entities;

namespace PlatformDemo.Data.Extensions
{
    public static class DbInitializer
    {
        public static async Task Initialize(this AppDbContext db)
        {
            Console.WriteLine("Initializing database");
            await db.Database.MigrateAsync();
            Console.WriteLine("Database initialized");

            if (! await db.Categories.AnyAsync())
            {
                Console.WriteLine("Seeding categories...");

                var categories = new List<Category>
                {
                    new Category { Name = "Music" },
                    new Category { Name = "Film" },
                    new Category { Name = "Literature" }
                };

                await db.Categories.AddRangeAsync(categories);
                await db.SaveChangesAsync();
            }

            if (! await db.Items.AnyAsync())
            {
                Console.WriteLine("Seeding items...");

                var music = await db.Categories
                    .FirstOrDefaultAsync(c => c.Name == "Music");

                var film = await db.Categories
                    .FirstOrDefaultAsync(c => c.Name == "Film");

                var lit = await db.Categories
                    .FirstOrDefaultAsync(c => c.Name == "Literature");

                var items = new List<Item>
                {
                    new Item { CategoryId = music.Id, Name = "Pink Floyd - The Dark Side of the Moon", Description = "Classic Rock" },
                    new Item { CategoryId = music.Id, Name = "Jimi Hendrix - Electric Ladyland", Description = "Classic Rock" },
                    new Item { CategoryId = music.Id, Name = "Nirvana - Nevermind", Description = "Grunge" },
                    new Item { CategoryId = music.Id, Name = "Chevelle - This Type of Thinking (Could Do Us In)", Description = "Rock" },
                    new Item { CategoryId = film.Id, Name = "The Mandalorian", Description = "A Star Wars Story" },
                    new Item { CategoryId = film.Id, Name = "The Simpsons", Description = "Classic TV Goodness" },
                    new Item { CategoryId = film.Id, Name = "Inception", Description = "Morty: Inception made sense. Rick: You don't have to try to impress me, Morty." },
                    new Item { CategoryId = film.Id, Name = "Joker", Description = "Disturbingly awesome" },
                    new Item { CategoryId = lit.Id, Name = "Norse Mythology - Neil Gaiman", Description = "Mythology & Folk Tales" },
                    new Item { CategoryId = lit.Id, Name = "1984 - George Orwell", Description = "Dystopian Fiction" },
                    new Item { CategoryId = lit.Id, Name = "The Currents of Space - Isaac Asimov", Description = "Science Fiction" },
                    new Item { CategoryId = lit.Id, Name = "Brave New World - Aldous Huxley", Description = "Classic American Literature" }
                };

                await db.Items.AddRangeAsync(items);
                await db.SaveChangesAsync();
            }

            if (!await db.Folders.AnyAsync())
            {
                Console.WriteLine("Seeding file system...");

                var folders = new List<Folder>
                {
                    new Folder
                    {
                        Name = "Jaime",
                        Color = "#304ffe",
                        Folders = new List<Folder>
                        {
                            new Folder
                            {
                                Name = "Images",
                                Color = "#2196f3",
                                Files = new List<File>
                                {
                                    new File
                                    {
                                        Name = "map.png",
                                        Length = 1024.RandomLong(),
                                        CreationTime = CoreExtensions.RandomDate()
                                    },
                                    new File
                                    {
                                        Name = "space.png",
                                        Length = 1024.RandomLong(),
                                        CreationTime = CoreExtensions.RandomDate()
                                    }
                                }
                            },
                            new Folder
                            {
                                Name = "Documents",
                                Color = "#00e676",
                                Files = new List<File>
                                {
                                    new File
                                    {
                                        Name = "audit.docx",
                                        Length = 1024.RandomLong(),
                                        CreationTime = CoreExtensions.RandomDate()
                                    },
                                    new File
                                    {
                                        Name = "JaimeStill.pdf",
                                        Length = 1024.RandomLong(),
                                        CreationTime = CoreExtensions.RandomDate()
                                    }
                                }
                            },
                            new Folder
                            {
                                Name = "Videos",
                                Color = "#f44336",
                                Files = new List<File>
                                {
                                    new File
                                    {
                                        Name = "inception.mp4",
                                        Length = 1024.RandomLong(),
                                        CreationTime = CoreExtensions.RandomDate()
                                    },
                                    new File
                                    {
                                        Name = "dr-sleep.mp4",
                                        Length = 1024.RandomLong(),
                                        CreationTime = CoreExtensions.RandomDate()
                                    }
                                }
                            }
                        }
                    },
                    new Folder
                    {
                        Name = "Phil",
                        Color = "#304ffe",
                        Folders = new List<Folder>
                        {
                            new Folder
                            {
                                Name = "Images",
                                Color = "#2196f3",
                                Files = new List<File>
                                {
                                    new File
                                    {
                                        Name = "motorcycle.png",
                                        Length = 1024.RandomLong(),
                                        CreationTime = CoreExtensions.RandomDate()
                                    },
                                    new File
                                    {
                                        Name = "carnival.png",
                                        Length = 1024.RandomLong(),
                                        CreationTime = CoreExtensions.RandomDate()
                                    }
                                }
                            },
                            new Folder
                            {
                                Name = "Documents",
                                Color = "#00e676",
                                Files = new List<File>
                                {
                                    new File
                                    {
                                        Name = "PhilipPerry.pdf",
                                        Length = 1024.RandomLong(),
                                        CreationTime = CoreExtensions.RandomDate()
                                    },
                                    new File
                                    {
                                        Name = "budget.xslx",
                                        Length = 1024.RandomLong(),
                                        CreationTime = CoreExtensions.RandomDate()
                                    }
                                }
                            },
                            new Folder
                            {
                                Name = "Videos",
                                Color = "#f44336",
                                Files = new List<File>
                                {
                                    new File
                                    {
                                        Name = "pineapple-express.mp4",
                                        Length = 1024.RandomLong(),
                                        CreationTime = CoreExtensions.RandomDate()
                                    },
                                    new File
                                    {
                                        Name = "the-report.mp4",
                                        Length = 1024.RandomLong(),
                                        CreationTime = CoreExtensions.RandomDate()
                                    }
                                }
                            }
                        }
                    },
                    new Folder
                    {
                        Name = "Mark",
                        Color = "#304ffe",
                        Folders = new List<Folder>
                        {
                            new Folder
                            {
                                Name = "Images",
                                Color = "#2196f3"
                            },
                            new Folder
                            {
                                Name = "Documents",
                                Color = "#00e676"
                            },
                            new Folder
                            {
                                Name = "Videos",
                                Color = "#f44336"
                            }
                        }
                    }
                };

                await db.Folders.AddRangeAsync(folders);
                await db.SaveChangesAsync();
            }
        }
    }
}