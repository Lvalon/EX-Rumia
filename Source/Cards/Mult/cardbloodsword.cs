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
using LBoL.Core.Cards;
using System;
using lvalonexrumia.GunName;

namespace lvalonexrumia.Cards
{
	public sealed class cardbloodswordDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = false;

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 0 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.SingleEnemy;

			config.Damage = 10;
			config.Value1 = 5; //Percentage of max HP to heal
			config.Value2 = 1; //token and bloodmark level consumed

			config.GunName = GunNameID.GetGunFromId(6162);
			config.GunNameBurst = GunNameID.GetGunFromId(6162);

			config.Keywords = Keyword.Exile | Keyword.AutoExile | Keyword.Accuracy | Keyword.Retain;
			config.UpgradedKeywords = Keyword.Exile | Keyword.AutoExile | Keyword.Accuracy | Keyword.Retain;

			config.RelativeEffects = new List<string>() { nameof(seincrease), nameof(sebloodmark), nameof(sebloodsword) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(seincrease), nameof(sebloodmark), nameof(sebloodsword) };

			config.Illustrator = "sarise";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodswordDef))]
	public sealed class cardbloodsword : lvalonexrumiaCard
	{
		public override bool OnExileVisual => false;
		public override IEnumerable<BattleAction> OnExile(CardZone srcZone)
		{
			if (Battle.BattleShouldEnd || srcZone != CardZone.Hand)
			{
				return null;
			}

			return LeaveHandReactor();
		}

		private IEnumerable<BattleAction> LeaveHandReactor()
		{
			if (Battle.BattleShouldEnd) { yield break; }
			yield return new PlayCardAction(this);
			yield break;
		}

		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);

		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return AttackAction(selector, GunName);
			EnemyUnit enemy = selector.SelectedEnemy;
			if (enemy.IsAlive && !Battle.BattleShouldEnd)
			{
				if (enemy.TryGetStatusEffect(out sebloodmark bloodmark))
				{
					if (IsUpgraded)
					{
						yield return new ApplyStatusEffectAction<sebloodmark>(enemy, Value2, 0, 0, 0, 0.2f);
					}
					else
					{
						bloodmark.Level -= Value2;
					}
					//bloodmark.Level += IsUpgraded ? Value2 : -Value2;
					if (bloodmark.Level <= 0)
					{
						yield return new RemoveStatusEffectAction(bloodmark, true, 0.1f);
					}
					if (!IsUpgraded)
					{
						yield return new ChangeLifeAction(heal);
					}
				}
				else
				{
					// if (IsUpgraded)
					// {
					// 	yield return new ApplyStatusEffectAction<sebloodmark>(enemy, Value2, 0, 0, 0, 0.2f);
					// }
					// else
					// {
					yield return new ApplyStatusEffectAction<sebloodmark>(enemy, Value2, 0, 0, 0, 0.2f);
					// }
				}
			}
			if (IsUpgraded)
			{
				yield return new ChangeLifeAction(heal);
			}
			yield break;
		}
	}
}


