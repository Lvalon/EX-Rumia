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
using lvalonexrumia.GunName;

namespace lvalonexrumia.Cards
{
	public sealed class carddevourDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Hybrid = 2, HybridColor = 7 };
			config.Rarity = Rarity.Uncommon;
			config.FindInBattle = false;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.SingleEnemy;

			config.GunName = GunNameID.GetGunFromId(7020);
			config.GunNameBurst = GunNameID.GetGunFromId(7021);

			config.Damage = 13;
			config.UpgradedDamage = 17;
			config.Value1 = 10; //max life percent
			config.Keywords = Keyword.Expel | Keyword.Accuracy;
			config.UpgradedKeywords = Keyword.Expel | Keyword.Accuracy;

			config.Illustrator = "c0sm1c0wl";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(carddevourDef))]
	public sealed class carddevour : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return AttackAction(selector, GunName);
			yield break;
		}
		protected override void OnEnterBattle(BattleController battle)
		{
			ReactBattleEvent(Battle.EnemyDied, OnEnemyDied);
		}
		private IEnumerable<BattleAction> OnEnemyDied(DieEventArgs args)
		{
			if (args.DieSource == this && !args.Unit.HasStatusEffect<Servant>())
			{
				NotifyActivating();
				GameRun.SetHpAndMaxHp(GameRun.Player.Hp + heal, GameRun.Player.MaxHp + heal, true);
				Card deckCardByInstanceId = GameRun.GetDeckCardByInstanceId(InstanceId);
				if (deckCardByInstanceId != null)
				{
					GameRun.RemoveDeckCard(deckCardByInstanceId, false);
				}
				yield return new RemoveCardAction(this);
			}
			yield break;
		}
	}
}


