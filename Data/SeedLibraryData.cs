using LibrarySystem.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Data;

public static class SeedLibraryData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

        context.Database.EnsureCreated();

        if (context.Books.Any())
        {
            return;
        }

        var seedBooks = new List<Book>
        {
            new Book
            {
                Title = "Saatleri Ayarlama Enstitüsü",
                Author = "Ahmet Hamdi Tanpınar",
                Category = "Novel",
                PublishedYear = 1961,
                Description = "A modern classic that critiques the flawed attitudes of Turkish society wavering between two civilizations (East and West) through the concepts of time and clocks, immortalized by the character Hayri İrdal.",
                TotalCopies = 3,
                AvailableCopies = 3
            },
            new Book
            {
                Title = "İnce Memed",
                Author = "Yaşar Kemal",
                Category = "Novel",
                PublishedYear = 1955,
                Description = "A masterpiece famous for its nature descriptions, telling the story of İnce Memed living in the Taurus Mountains, his rebellion against the Agha who exploits the villagers, and his transformation into a bandit legend.",
                TotalCopies = 2,
                AvailableCopies = 2
            },
            new Book
            {
                Title = "Kürk Mantolu Madonna",
                Author = "Sabahattin Ali",
                Category = "Novel",
                PublishedYear =1943,
                Description = "A sorrowful story recounting Raif Efendi's passionate love affair that begins with a painting he sees in an art gallery in Germany, followed by the deep loneliness he experiences afterward.",
                TotalCopies = 1,
                AvailableCopies = 1
            },
            new Book
            {
                Title = "Tutunamayanlar",
                Author = "Oğuz Atay",
                Category = "Novel",
                PublishedYear = 1972,
                Description = "Through the story of Turgut Özben investigating the past of his friend who committed suicide, this work ironically narrates the inner world of intellectuals who cannot adapt to social norms and the petty-bourgeois world..",
                TotalCopies = 2,
                AvailableCopies = 2
            },
            new Book
            {
                Title = "Puslu Kıtalar Atlası",
                Author = "İhsan Oktay Anar",
                Category = "Historical Fantasy / Philosophical Fiction",
                PublishedYear = 1995,
                Description = "A fairy-tale-like novel set in 17th-century Istanbul, intertwining dreams, reality, and philosophy, centered around the world atlas prepared by Uzun İhsan Efendi and the adventures of his son, Bünyamin.",
                TotalCopies = 2,
                AvailableCopies = 2
            },
            new Book
            {
                Title = "Benim Adım Kırmızı",
                Author = "Orhan Pamuk",
                Category = "Historical Novel / Mystery",
                PublishedYear = 1998,
                Description = "A polyphonic work set in 1591 Ottoman Istanbul, narrating a murder committed among miniature artists and the clash of Eastern and Western art philosophies through the voices of different characters (even a coin and a color).",
                TotalCopies = 1,
                AvailableCopies = 1
            },
            new Book
            {
                Title = "İstanbul Hatırası",
                Author = "Ahmet Ümit",
                Category = "Detective / Crime",
                PublishedYear = 2010,
                Description = "A gripping detective novel where Chief Inspector Nevzat explores the city's thousands of years of history (from Byzantion to the Ottomans) while solving murders committed on Istanbul's historical peninsula.",
                TotalCopies = 2,
                AvailableCopies = 2
            },
            new Book
            {
                Title = "Suyu Arayan Adam",
                Author = "Şevket Süreyya Aydemir",
                Category = "Autobiography / History",
                PublishedYear = 1959,
                Description = "The story of a generation told through the author's own life, covering the collapse of the Ottoman Empire, the War of Independence, and the idealism during the founding years of the Republic..",
                TotalCopies = 2,
                AvailableCopies = 2
            },
            new Book
            {
                Title = "Bu Ülke",
                Author = "Cemil Meriç",
                Category = "Essay / Thought",
                PublishedYear = 1974,
                Description = "One of the most important texts of Turkish intellectual history, deeply examining the identity crisis of the Turkish intellectual, the East-West conflict, right-left ideologies, and language issues.",
                TotalCopies = 1,
                AvailableCopies = 1
            },
            new Book
            {
                Title = "İnsan Olmak",
                Author = "Engin Geçtan",
                Category = "Psychology",
                PublishedYear = 1983,
                Description = "A bedside guide that explains modern humans' existential problems such as loneliness, feelings of worthlessness, anger, and responsibility in plain language accessible to everyone.",
                TotalCopies = 2,
                AvailableCopies = 2
            },
            new Book
            {
                Title = "Bir Ömür Nasıl Yaşanır?",
                Author = "İlber Ortaylı",
                Category = "Interview / Personal Development",
                PublishedYear = 2019,
                Description = "A guide where the famous historian offers wise advice to young people on travel, education, reading habits, and life culture, based on his own life experiences.",
                TotalCopies = 1,
                AvailableCopies = 1
            },
            new Book
            {
                Title = "Yaşar Ne Yaşar Ne Yaşamaz",
                Author = "Aziz Nesin",
                Category = "Humor / Satire",
                PublishedYear = 1977,
                Description = "Critiques bureaucracy through the tragicomic story of Yaşar Yaşamaz, who is seen as not living by the state because he has no ID card, yet is considered living when it comes to duties like military service and taxes.",
                TotalCopies = 1,
                AvailableCopies = 1
            },
            new Book
            {
                Title = "Semaver",
                Author = "Sait Faik Abasıyanık",
                Category = "Short Story",
                PublishedYear = 1936,
                Description = "Consists of stories that describe Istanbul's ordinary people, fishermen, workers, and small details of daily life with great human love and poetic language.",
                TotalCopies = 1,
                AvailableCopies = 1
            },
            new Book
            {
                Title = "Sevda Sözleri",
                Author = "Cemal Süreya",
                Category = "Poetry",
                PublishedYear = 1984,
                Description = "A comprehensive work collecting all the poems of Cemal Süreya, one of the pioneers of the Second New poetry movement, processing love, sexuality, melancholy, and irony with unique lyricism.",
                TotalCopies = 2,
                AvailableCopies = 2
            },
            new Book
            {
                Title = "Son Ada",
                Author = "Zülfü Livaneli",
                Category = "Political Allegory / Dystopia",
                PublishedYear = 2008,
                Description = "A striking metaphor telling how a retired dictator who settles on an isolated, peaceful island suppresses first nature and then people, dragging the society into chaos.",
                TotalCopies = 2,
                AvailableCopies = 2
            }
        };

        context.Books.AddRange(seedBooks);

        context.SaveChanges();
    }
}
