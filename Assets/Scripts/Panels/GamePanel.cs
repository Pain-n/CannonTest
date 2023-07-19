using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    private GameContext context;
    private UpgradeListSO upgradeList;

    public TextMeshProUGUI ValletText;

    public TextMeshProUGUI MonstersCountText;

    public TextMeshProUGUI DifficultyText;

    public TextMeshProUGUI ScoreText;

    public Button FreezeSpawnBoostButton;
    public TextMeshProUGUI FreezeSpawnBoostButtonText;

    public Button AnnihilationBoostButton;
    public TextMeshProUGUI AnnihilationBoostButtonText;

    public Button UpgradeAttackSpeedButton;
    public TextMeshProUGUI UpgradeAttackSpeedButtonText;

    public Button UpgradeDamageButton;
    public TextMeshProUGUI UpgradeDamageButtonText;

    private Dictionary<BoostType, int> BoostsDict = new Dictionary<BoostType, int>()
    {
        {BoostType.Freeze,3},
        {BoostType.Annihilation,3}
    };

    public void Init(GameContext _context)
    {
        GameContext.UpdateVallet += UpdateValletUI;
        GameContext.UpdateEnemiesCount += UpdateEnemiesCountUI;
        GameContext.UpdateDifficulty += UpdateDifficultyUI;
        GameContext.UpdateScore += UpdateScoreUI;

        context = _context;

        upgradeList = (UpgradeListSO)Resources.Load<UpgradeListSO>("SO/UpgradeList").Clone();

        UpdateValletUI();
        UpdateEnemiesCountUI();
        UpdateScoreUI();

        for (int i = 0; i < upgradeList.UpgradeList.Count; i++)
        {
            if (upgradeList.UpgradeList[i].UpgradeType == UpgradeType.AttackSpeed)
            {
                UpgradeAttackSpeedButtonText.text = $"Attack speed:{context.Cannon.AttackCooldown} \n Upgrade cost:{upgradeList.UpgradeList[i].UpgradeStages[upgradeList.UpgradeList[i].CurrentLvl].Cost}";
            }
            if (upgradeList.UpgradeList[i].UpgradeType == UpgradeType.Damage)
            {
                UpgradeDamageButtonText.text = $"Damage:{context.Cannon.Damage} \n Upgrade cost:{upgradeList.UpgradeList[i].UpgradeStages[upgradeList.UpgradeList[i].CurrentLvl].Cost}";
            }
        }

        FreezeSpawnBoostButtonText.text = $"Freeze enemy spawn \n Charges: {BoostsDict[BoostType.Freeze]}";
        AnnihilationBoostButtonText.text = $"Annihilate enemies \n Charges: {BoostsDict[BoostType.Annihilation]}";


        FreezeSpawnBoostButton.onClick.AddListener(() =>
        {
            FreezeSpawn(3);
        });

        AnnihilationBoostButton.onClick.AddListener(() =>
        {
            AnnihilateEnemies();
        });

        UpgradeAttackSpeedButton.onClick.AddListener(() =>
        {
            UpgradeAttackSpeed();
        });

        UpgradeDamageButton.onClick.AddListener(() =>
        {
            UpgradeDamage();
        });
    }

    private void UpgradeAttackSpeed()
    {
        for (int i = 0; i < upgradeList.UpgradeList.Count; i++)
        {
            if (upgradeList.UpgradeList[i].UpgradeType == UpgradeType.AttackSpeed)
            {
                if (upgradeList.UpgradeList[i].CurrentLvl >= upgradeList.UpgradeList[i].UpgradeStages.Count) break;
                if (upgradeList.UpgradeList[i].UpgradeStages[upgradeList.UpgradeList[i].CurrentLvl].Cost > context.Vallet) break;


                context.Vallet -= upgradeList.UpgradeList[i].UpgradeStages[upgradeList.UpgradeList[i].CurrentLvl].Cost;
                context.Cannon.AttackCooldown = upgradeList.UpgradeList[i].UpgradeStages[upgradeList.UpgradeList[i].CurrentLvl].Value;


                if (upgradeList.UpgradeList[i].CurrentLvl + 1 < upgradeList.UpgradeList[i].UpgradeStages.Count)
                    UpgradeAttackSpeedButtonText.text = $"Attack speed:{context.Cannon.AttackCooldown} \n Upgrade cost:{upgradeList.UpgradeList[i].UpgradeStages[upgradeList.UpgradeList[i].CurrentLvl + 1].Cost}";
                else
                    UpgradeAttackSpeedButtonText.text = $"Attack speed:{context.Cannon.AttackCooldown} \n Max";

                upgradeList.UpgradeList[i].CurrentLvl++;
                UpdateValletUI();
                break;
            }
        }
    }

    private void UpgradeDamage()
    {
        for (int i = 0; i < upgradeList.UpgradeList.Count; i++)
        {
            if (upgradeList.UpgradeList[i].UpgradeType == UpgradeType.Damage)
            {
                if (upgradeList.UpgradeList[i].CurrentLvl >= upgradeList.UpgradeList[i].UpgradeStages.Count) break;
                if (upgradeList.UpgradeList[i].UpgradeStages[upgradeList.UpgradeList[i].CurrentLvl].Cost > context.Vallet) break;


                context.Vallet -= upgradeList.UpgradeList[i].UpgradeStages[upgradeList.UpgradeList[i].CurrentLvl].Cost;
                context.Cannon.Damage = (int)upgradeList.UpgradeList[i].UpgradeStages[upgradeList.UpgradeList[i].CurrentLvl].Value;


                if (upgradeList.UpgradeList[i].CurrentLvl + 1 < upgradeList.UpgradeList[i].UpgradeStages.Count)
                    UpgradeDamageButtonText.text = $"Damage:{context.Cannon.Damage} \n Upgrade cost:{upgradeList.UpgradeList[i].UpgradeStages[upgradeList.UpgradeList[i].CurrentLvl + 1].Cost}";
                else
                    UpgradeDamageButtonText.text = $"Damage:{context.Cannon.AttackCooldown} \n Max";

                upgradeList.UpgradeList[i].CurrentLvl++;
                UpdateValletUI();
                break;
            }
        }
    }

    private void AnnihilateEnemies()
    {
        if (BoostsDict[BoostType.Annihilation] <= 0) return;

        while (context.EnemiesOnField.Count > 0)
        {
            for (int i = 0; i < context.EnemiesOnField.Count; i++)
            {
                context.EnemiesOnField[i].Death();
            }
        }
        BoostsDict[BoostType.Annihilation]--;
        AnnihilationBoostButtonText.text = $"Annihilate enemies \n Charges: {BoostsDict[BoostType.Annihilation]}";
    }

    private void FreezeSpawn(float seconds)
    {
        if (BoostsDict[BoostType.Freeze] <= 0) return;

        context.BeginCorounine(context.FreezeSpawnEnemyCycle(seconds));

        BoostsDict[BoostType.Freeze]--;
        FreezeSpawnBoostButtonText.text = $"Freeze enemy spawn \n Charges: {BoostsDict[BoostType.Freeze]}";
    }

    public void UpdateValletUI()
    {
        ValletText.text = $"Gold:{context.Vallet}";
    }

    public void UpdateEnemiesCountUI()
    {
        MonstersCountText.text = $"Monsters on the field: {context.EnemiesOnField.Count}";
    }

    public void UpdateScoreUI()
    {
        ScoreText.text = $"Score: {context.Score}";
    }

    public void UpdateDifficultyUI()
    {
        DifficultyText.text = $"Difficulty lvl: {context.DifficultyID + 1}";
    }

    private void OnDisable()
    {
        GameContext.UpdateVallet -= UpdateValletUI;
        GameContext.UpdateEnemiesCount -= UpdateEnemiesCountUI;
        GameContext.UpdateDifficulty -= UpdateDifficultyUI;
        GameContext.UpdateScore -= UpdateScoreUI;
    }
}

public enum BoostType
{
    Freeze,
    Annihilation
}
