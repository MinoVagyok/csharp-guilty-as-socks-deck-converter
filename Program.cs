// Program.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DeckConverter
{
    // JSON struktúra leképezése
    public class Deck
    {
        [JsonPropertyName("deckName")]
        public string DeckName { get; set; }

        [JsonPropertyName("isValid")]
        public bool IsValid { get; set; }

        [JsonPropertyName("proofs")]
        public List<Proof> Proofs { get; set; }
    }

    public class Proof
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("tagline")]
        public string Tagline { get; set; }

        [JsonPropertyName("cardType")]
        public int CardType { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 1) Beolvasás
            var path = "exported_deck.txt";
            if (!File.Exists(path))
            {
                Console.Error.WriteLine($"Nem találom a fájlt: {path}");
                return;
            }

            string json = File.ReadAllText(path);
            var deck = JsonSerializer.Deserialize<Deck>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (deck == null || deck.Proofs == null)
            {
                Console.Error.WriteLine("Hiba a JSON feldolgozásakor.");
                return;
            }

            // 2) Kimenet konzolra
            foreach (var p in deck.Proofs)
            {
                // két sor szöveg: content és (tagline)
                string line1 = p.Content;
                string line2 = $"({p.Tagline})";

                // Dinamikus szélesség: a leghosszabb sor + 2 szóköz padding
                int maxInner = Math.Max(line1.Length, line2.Length);
                int boxWidth = maxInner + 2;

                // Felső keret
                Console.WriteLine("╔" + new string('═', boxWidth) + "╗");
                // Tartalom
                Console.WriteLine("║ " + line1.PadRight(boxWidth - 1) + "║");
                // Felirat
                Console.WriteLine("║ " + line2.PadRight(boxWidth - 1) + "║");
                // Alsó keret
                Console.WriteLine("╚" + new string('═', boxWidth) + "╝");
                Console.WriteLine();
            }

            Console.WriteLine($"Összesen {deck.Proofs.Count} kártya.");
        }
    }
}

