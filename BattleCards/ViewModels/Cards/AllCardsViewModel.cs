using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCards.ViewModels.Cards
{
    public class AllCardsViewModel
    {
        public ICollection<CardsViewModel> Cards { get; set; }
    }
}
