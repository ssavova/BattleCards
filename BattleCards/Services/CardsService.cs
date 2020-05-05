using BattleCards.Data;
using BattleCards.Models;
using BattleCards.ViewModels.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCards.Services
{
    public class CardsService : ICardsService
    {
        private readonly ApplicationDbContext db;

        public CardsService(ApplicationDbContext context)
        {
            this.db = context;

        }

        public void AddCardToUserCollection(int cardId, string userId)
        {
            var newUserCard = new UserCard
            {
                CardId = cardId,
                UserId = userId,
            };

            this.db.UserCards.Add(newUserCard);
            this.db.SaveChanges();
        }

        public void CreateCard(string name, string image, string keyword, int attack, int health, string description)
        {
            var card = new Card()
            {
                Name = name,
                ImageUrl = image,
                Keyword = keyword,
                Attack = attack,
                Health = health,
                Description = description,
            };

            this.db.Cards.Add(card);
            this.db.SaveChanges();
        }

        public List<CardsViewModel> GetAllCards()
        {
            var collection = this.db.Cards.Select(c => new CardsViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Health = c.Health,
                Attack = c.Attack,
                ImageUrl = c.ImageUrl,
                Keyword = c.Keyword,
            }).ToList();

            return collection;
        }

        public bool IsUserHasSuchCard(int cardId, string userId)
        {
            return this.db.UserCards.Any(c => c.CardId == cardId && c.UserId == userId);
        }

        public void RemoveCardFromUserCollection(int cardId, string userId)
        {
            var userCards = this.db.UserCards.FirstOrDefault(uc => uc.CardId == cardId && uc.UserId == userId);

            this.db.UserCards.Remove(userCards);
            this.db.SaveChanges();
        }

        public Card ReturnNewlyCreatedCard(string name)
        {
            return this.db.Cards.FirstOrDefault(c => c.Name == name);
        }

        public List<CardsViewModel> ReturnUserCollectioncards(string userId)
        {
            var collection = this.db.UserCards.Where(u => u.UserId == userId).Select(c => new CardsViewModel()
            {
                ImageUrl = c.Card.ImageUrl,
                Id = c.Card.Id,
                Name = c.Card.Name,
                Keyword = c.Card.Keyword,
                Description = c.Card.Description,
                Attack = c.Card.Attack,
                Health = c.Card.Health,
            }).ToList();

            return collection;
        }
    }
}
