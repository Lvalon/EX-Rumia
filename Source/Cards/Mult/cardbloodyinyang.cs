using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonexrumia.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Units;
using lvalonexrumia.StatusEffects;
using lvalonexrumia.Patches;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using LBoL.Presentation;
using LBoL.Core.StatusEffects;
using LBoL.Core.Cards;
using LBoL.EntityLib.StatusEffects.Neutral.TwoColor;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.Core.Battle.Interactions;
using LBoL.EntityLib.Cards.Character.Sakuya;
using System.Linq;

namespace lvalonexrumia.Cards
{
	public sealed class cardbloodyinyangDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Red = 1, Black = 1, Hybrid = 3, HybridColor = 7 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Nobody;

			config.Keywords = Keyword.Retain | Keyword.Exile;
			config.UpgradedKeywords = Keyword.Retain | Keyword.Exile | Keyword.Echo;

			config.Value1 = 2; //exile cards to return

			config.Illustrator = "Koissa";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodyinyangDef))]
	public sealed class cardbloodyinyang : YinyangCardBase
	{
		public int truevalue
		{
			get
			{
				var player = GameMaster.Instance.CurrentGameRun?.Battle?.Player;
				var se = player?.GetStatusEffect<YinyangQueenSe>();
				return se != null ? Value1 * (se.Level + 1) : Value1;
			}
		}
		public override Interaction Precondition()
		{
			if (Battle.ExileZone.Count <= 0)
			{
				return null;
			}
			return new SelectCardInteraction(0, truevalue, Battle.ExileZone);
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			if (precondition != null)
			{
				Card[] cards = ((SelectCardInteraction)precondition)?.SelectedCards.ToArray();
				foreach (Card card in cards)
				{
					if (card != null)
					{
						yield return new MoveCardAction(card, CardZone.Hand);
					}
					//SakuyaFuneral
				}
			}
			yield return UpgradeAllHandsAction();
			yield break;
		}

	}
}


