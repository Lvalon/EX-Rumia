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
using System.Linq;
using System;

namespace lvalonexrumia.Cards
{
	public sealed class cardkaleidoDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Blue, ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Red = 1, Blue = 1, Black = 1 };
			config.Rarity = Rarity.Rare;
			config.IsXCost = true;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.All;

			config.Value1 = 5; //max life percent
			config.Value2 = 5; //min life
			config.Keywords = Keyword.Expel | Keyword.Synergy;
			config.UpgradedKeywords = Keyword.Expel | Keyword.Replenish | Keyword.Retain | Keyword.Synergy;

			config.RelativeEffects = new List<string>() { nameof(sedecrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease) };

			config.Illustrator = "ツェぺシュ";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardkaleidoDef))]
	public sealed class cardkaleido : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		int heals = 0;
		bool activating = false;
		public override ManaGroup GetXCostFromPooled(ManaGroup pooledMana)
		{
			ManaGroup empty = ManaGroup.Empty;
			foreach (ManaColor singleColor in ManaColors.SingleColors)
			{
				if (pooledMana.GetValue(singleColor) > 0 && base.XCostRequiredMana.GetValue(singleColor) == 0)
				{
					empty += ManaGroup.Single(singleColor);
				}
			}

			empty.Philosophy = pooledMana.Philosophy;
			int num = 5;
			if (empty.Amount > num)
			{
				empty -= ManaGroup.Philosophies(empty.Amount - num);
			}

			return empty;
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			activating = true;
			var usedColors = ManaColors.SingleColors
				.Where(color => consumingMana.GetValue(color) > 0)
				.ToList();

			int extraColors = consumingMana.Philosophy + base.GameRun.SynergyAdditionalCount;
			for (int i = 1; i < extraColors; i++)
			{
				var unused = ManaColors.SingleColors.Except(usedColors).FirstOrDefault();
				if (unused != default)
				{
					usedColors.Add(unused);
				}
			}

			if (extraColors >= 1)
			{
				usedColors.Add(ManaColor.Philosophy);
			}

			if (!usedColors.Any())
			{
				yield break;
			}

			heals = usedColors.Count;
			foreach (var _ in usedColors)
			{
				foreach (Unit unit in Battle.AllAliveUnits)
				{
					if (!Battle.BattleShouldEnd && unit.IsAlive)
					{
						if (unit == Battle.Player)
						{
							yield return new ChangeLifeAction(-toolbox.hpfrompercent(unit, Value1, false));
						}
						else
						{
							yield return new ChangeLifeAction(-Math.Max(Value2, toolbox.hpfrompercent(unit, Value1, true)), unit);
						}
					}
				}
			}
			activating = false;
		}
		protected override void OnEnterBattle(BattleController battle)
		{
			heals = 0;
			ReactBattleEvent(Battle.EnemyDied, OnEnemyDied);
		}
		private IEnumerable<BattleAction> OnEnemyDied(DieEventArgs args)
		{
			//if (args.DieSource == this && !args.Unit.HasStatusEffect<Servant>())
			if (activating && !args.Unit.HasStatusEffect<Servant>())
			{
				NotifyActivating();
				GameRun.SetHpAndMaxHp(GameRun.Player.Hp + heals, GameRun.Player.MaxHp + heals, true);
				// Card deckCardByInstanceId = GameRun.GetDeckCardByInstanceId(InstanceId);
				// if (deckCardByInstanceId != null)
				// {
				// 	GameRun.RemoveDeckCard(deckCardByInstanceId, false);
				// }
				// yield return new RemoveCardAction(this);
			}
			yield break;
		}
	}
}


