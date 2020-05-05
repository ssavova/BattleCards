using BattleCards.Models;
using BattleCards.ViewModels.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCards.Services
{
    public interface ICardsService
    {
        void CreateCard(string name, string image, string keyword, int attack, int health, string description);

        List<CardsViewModel> GetAllCards();

        void AddCardToUserCollection(int cardId, string userId);

        bool IsUserHasSuchCard(int cardId, string userId);
        void RemoveCardFromUserCollection(int cardId, string userId);

        List<CardsViewModel> ReturnUserCollectioncards(string userId);

        Card ReturnNewlyCreatedCard(string name);
    }
}
