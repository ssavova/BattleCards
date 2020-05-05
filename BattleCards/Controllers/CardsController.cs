using BattleCards.Services;
using BattleCards.ViewModels.Cards;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private readonly ICardsService cardsService;
       
        public CardsController(CardsService service)
        {
            this.cardsService = service;
        }

        public HttpResponse Add()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(CreateCardInputModel input)
        {
            if (input.Name?.Length < 5 || input.Name?.Length > 20)
            {
                return this.Redirect("/Cards/Add");
            }

            if (string.IsNullOrWhiteSpace(input.Image))
            {
                return this.Redirect("/Cards/Add");
            }

            if (input.Attack < 0)
            {
                return this.Redirect("/Cards/Add");
            }

            if (input.Health < 0)
            {
                return this.Redirect("/Cards/Add");
            }

            if (string.IsNullOrWhiteSpace(input.Description) || input.Description.Length > 200)
            {
                return this.Redirect("/Cards/Add");
            }

            this.cardsService.CreateCard(input.Name, input.Image, input.Keyword, input.Attack, input.Health, input.Description);

            var newlyCreatedCard = this.cardsService.ReturnNewlyCreatedCard(input.Name);
            this.cardsService.AddCardToUserCollection(newlyCreatedCard.Id, this.User);

            return this.Redirect("/Cards/All");
        }
        public HttpResponse All()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            var viewModel = new AllCardsViewModel() { Cards = this.cardsService.GetAllCards() };
            return this.View(viewModel);
        }

        public HttpResponse Collection()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = new AllCardsViewModel() { Cards = this.cardsService.ReturnUserCollectioncards(this.User) };
            return this.View(viewModel);
        }

        public HttpResponse AddToCollection(int cardId) 
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (this.cardsService.IsUserHasSuchCard(cardId,this.User)== false)
            {
                this.cardsService.AddCardToUserCollection(cardId, this.User);

                return this.Redirect("/Cards/All");
            }

            return this.Redirect("/Cards/All");
        }

        public HttpResponse RemoveFromCollection(int cardId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.cardsService.RemoveCardFromUserCollection(cardId, this.User);

            return this.Redirect("/Cards/Collection");
        }
    }
}
