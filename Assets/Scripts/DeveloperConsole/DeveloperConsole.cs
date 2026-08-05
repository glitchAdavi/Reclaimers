using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DeveloperConsole : MonoBehaviour
{
    public GameObject _devConsole;
    public bool consoleEnabled = false;

    public TMP_InputField _devConsoleInput;
    public TMP_Text _devConsoleText;

    public delegate string Command(string[] args);

    private Dictionary<string, Command> _consoleLogic = new Dictionary<string, Command>();
    private Dictionary<string, string> _consoleHelp = new Dictionary<string, string>();

    private void OnEnable()
    {
        _devConsole = Instantiate(GameManager.Instance.gameInfo.devConsolePrefab, GameObject.Find("Canvas").transform);
        _devConsole.transform.SetAsFirstSibling();
        _devConsoleInput = _devConsole.GetComponent<ConsoleHelper>().consoleInput;
        _devConsoleText = _devConsole.GetComponent<ConsoleHelper>().consoleText;

        _devConsole.SetActive(consoleEnabled);

        AddCommand("help", Help, "help [cmd] || List command/s info");
        AddCommand("cls", Cls, "cls || Clear console");
        AddCommand("close", Close, "close || Close console");
        AddCommand("give", Give, "Multiple variations:" +
            "\ngive xp [x] || Give player [x] xp" +
            "\ngive accxp [x] || Give account [x] xp" +
            "\ngive level [x] [silent=false] || Give player [x] levels, [silent] skips the upgrade selection menu and no upgrade is given" +
            "\ngive acclvl [x] || Give account [x] lvls" +
            "\ngive money [x] || Give player [x] money" +
            "\ngive upgrade [x] [stacks=1] || Give player [stacks] of [x] upgrade" +
            "\ngive weapon [x] || Give player [x] weapon" +
            "\ngive ability [x] || Give player [x] ability" +
            "\ngive item [x] || Give player [x] item");
        AddCommand("list", List, "Multiple variations:" +
            "\nlist weapon || List all weapons" +
            "\nlist upgrade || List all upgrades" +
            "\nlist ability || List all abilities" +
            "\nlist item || List all items");
        //AddCommand("save", SaveGame, "save || Saves the current state of the player");
        AddCommand("save", SaveAndExit, "save || Saves the current state of the player and exits to menu");
        AddCommand("load", LoadGame, "load || Reloads the player and returns to menu");
        AddCommand("reset", ResetSave, "reset || Reset the player and returns to menu");
        AddCommand("nuke", Nuke, "nuke || Kill all (non-boss) enemies in scene");
        AddCommand("magnet", Magnet, "magnet || Pull all xp and money in scene");
        AddCommand("win", TriggerWin, "win || Trigger win condition");
        AddCommand("lose", TriggerLose, "lose || Trigger lose condition");
        AddCommand("boss", Boss, "boss || Spawn random boss");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            consoleEnabled = !consoleEnabled;
            _devConsoleText.text = "> For all commands type 'help'";
            _devConsole.SetActive(consoleEnabled);
            GameManager.Instance.eventSystem.EnableAllControls(!consoleEnabled);
            _devConsoleInput.ActivateInputField();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_devConsoleInput.text != null)
            {
                Process(_devConsoleInput.text);
                _devConsoleInput.text = "";
                _devConsoleInput.ActivateInputField();
            }
        }
    }

    public void AddCommand(string key, Command commandEvent, string help)
    {
        if (!_consoleLogic.ContainsKey(key)) _consoleLogic.Add(key, commandEvent);
        else _consoleLogic[key] += commandEvent;

        if (!_consoleHelp.ContainsKey(key)) _consoleHelp.Add(key, help);
    }

    public void Process(string key)
    {
        char[] delim = new char[] { ' ' };
        string[] result = key.Split(delim);

        string command = result[0];
        string[] args = result.Skip(1).ToArray();

        if (_consoleLogic.ContainsKey(command))
        {
            string commandResult = $"\n> {_consoleLogic[command](args)}";
            if (!commandResult.Equals("\n> ")) _devConsoleText.text += commandResult;
        }
        else _devConsoleText.text += "\n> " + "ERROR: command not found";
    }

    public void Close()
    {
        consoleEnabled = !consoleEnabled;
        _devConsole.SetActive(consoleEnabled);
        GameManager.Instance.eventSystem.EnableAllControls(!consoleEnabled);
    }

    string Cls(string[] args)
    {
        if (args.Length > 0) return "ERROR: Too many parameters";

        _devConsoleText.text = "> For all commands type 'help'";
        return "";
    }

    string Help(string[] args)
    {
        if (args.Length > 1) return "ERROR: Too many parameters";

        if (args.Length > 0)
        {
            if (_consoleHelp.ContainsKey(args[0]))
            {
                _devConsoleText.text += "\n> -------------------------";
                _devConsoleText.text += $"\n> {_consoleHelp[args[0]]}";
                _devConsoleText.text += "\n> -------------------------";
            }

        }
        else
        {
            _devConsoleText.text += "\n> -------------------------";

            foreach (var c in _consoleLogic)
                _devConsoleText.text += $"\n> {c.Key}";

            _devConsoleText.text += "\n> -------------------------";
        }
        return "";
    }

    string Close(string[] args)
    {
        if (args.Length > 0) return "ERROR: Too many parameters";

        consoleEnabled = !consoleEnabled;
        _devConsole.SetActive(consoleEnabled);
        GameManager.Instance.eventSystem.EnableAllControls(!consoleEnabled);
        return "";
    }

    string Give(string[] args)
    {
        if (args.Length < 1) return "ERROR: Lack parameter";

        if (args[0].Equals("xp"))
        {
            return GiveXp(args.Skip(1).ToArray());
        }

        if (args[0].Equals("accxp"))
        {
            return GiveAccXp(args.Skip(1).ToArray());
        }

        if (args[0].Equals("level"))
        {
            return GiveLevel(args.Skip(1).ToArray());
        }

        if (args[0].Equals("acclvl"))
        {
            return GiveAccLvl(args.Skip(1).ToArray());
        }

        if (args[0].Equals("money"))
        {
            return GiveMoney(args.Skip(1).ToArray());
        }

        if (args[0].Equals("weapon"))
        {
            return GiveWeapon(args.Skip(1).ToArray());
        }

        if (args[0].Equals("upgrade"))
        {
            return GiveUpgrade(args.Skip(1).ToArray());
        }

        if (args[0].Equals("ability"))
        {
            return GiveAbility(args.Skip(1).ToArray());
        }

        if (args[0].Equals("item"))
        {
            return GiveItem(args.Skip(1).ToArray());
        }

        return "";
    }

    string GiveXp(string[] args)
    {
        if (args.Length < 1) return "ERROR: Quantity needed";
        if (args.Length > 1) return "ERROR: Too many parameters";

        float result;
        if (float.TryParse(args[0], out result))
        {
            GameManager.Instance.eventSystem.GivePlayerXp(result);
            return $"{result} xp given to Player";
        }
        else
        {
            return "ERROR: give xp [x] || x has to be a number";
        }
    }

    string GiveAccXp(string[] args)
    {
        if (args.Length < 1) return "ERROR: Quantity needed";
        if (args.Length > 1) return "ERROR: Too many parameters";

        float result;
        if (float.TryParse(args[0], out result))
        {
            GameManager.Instance.GiveAccountXp(result);
            return $"{result} xp given to Account";
        }
        else
        {
            return "ERROR: give accxp [x] || x has to be a number";
        }
    }

    string GiveLevel(string[] args)
    {
        if (args.Length < 1) return "ERROR: Quantity needed";
        if (args.Length > 2) return "ERROR: Too many parameters";

        int result;
        if (int.TryParse(args[0], out result))
        {
            bool skipLevel = false;
            if (args.Length > 1 && bool.TryParse(args[1], out skipLevel)) GameManager.Instance.eventSystem.GivePlayerLevel(result, skipLevel);
            else GameManager.Instance.eventSystem.GivePlayerLevel(result);
            return $"{result} levels given to Player";
        }
        else
        {
            return "ERROR: give level [x] || x has to be a number";
        }
    }

    string GiveAccLvl(string[] args)
    {
        if (args.Length < 1) return "ERROR: Quantity needed";
        if (args.Length > 1) return "ERROR: Too many parameters";

        int result;
        if (int.TryParse(args[0], out result))
        {
            GameManager.Instance.GiveAccountLevel(result);
            return $"{result} lvls given to Account";
        }
        else
        {
            return "ERROR: give xp [x] || x has to be a number";
        }
    }

    string GiveMoney(string[] args)
    {
        if (args.Length < 1) return "ERROR: Quantity needed";
        if (args.Length > 1) return "ERROR: Too many parameters";

        if (float.TryParse(args[0], out float result))
        {
            GameManager.Instance.eventSystem.GivePlayerMoney(result);
            return $"{result} money given to Player";
        }
        else
        {
            return "ERROR: give money [x] || x has to be a number";
        }
    }

    string GiveWeapon(string[] args)
    {
        if (args.Length < 1) return "ERROR: Weapon name needed";
        if (args.Length > 1) return "ERROR: Too many parameters";

        GameManager.Instance.playerPawn.SetWeapon(args[0], true);

        return $"Given weapon {args[0]} to player";
    }

    string GiveUpgrade(string[] args)
    {
        if (args.Length < 1) return "ERROR: Upgrade name needed";
        if (args.Length > 2) return "ERROR: Too many parameters";

        Upgrade u = GameManager.Instance.GetUpgrade(args[0]);
        if (u == null)
        {
            return $"ERROR: {args[0]} not an upgrade";
        }

        if (args.Length == 1)
        {
            GameManager.Instance.playerPawn.AddUpgrade(u, 1);
            return $"Given upgrade {args[0]} to player";
        }

        if (int.TryParse(args[1], out int stacks))
        {
            GameManager.Instance.playerPawn.AddUpgrade(u, stacks);
            return $"Given {stacks} of upgrade {args[0]} to player";
        }
        else
        {
            return $"ERROR: give upgrade {GameManager.Instance.GetUpgrade(args[0])} [stacks] || [stacks] has to be an int";
        }
    }

    string GiveAbility(string[] args)
    {
        if (args.Length < 1) return "ERROR: Ability name needed";
        if (args.Length > 1) return "ERROR: Too many parameters";

        GameManager.Instance.playerPawn.SetAbility(args[0]);

        return $"Given ability {args[0]} to player";
    }

    string GiveItem(string[] args)
    {
        if (args.Length < 1) return "ERROR: Ability name needed";
        if (args.Length > 1) return "ERROR: Too many parameters";

        //GameManager.Instance.playerPawn.AddItem(GameManager.Instance.GetItem(args[0]));

        return $"Given ability {args[0]} to player";
    }

    string List(string[] args)
    {
        if (args.Length != 1) return "ERROR: Wrong parameters";

        if (args[0].Equals("weapon"))
        {
            return ListWeapon();
        }

        if (args[0].Equals("upgrade"))
        {
            return ListUpgrade();
        }

        if (args[0].Equals("ability"))
        {
            return ListAbility();
        }

        if (args[0].Equals("item"))
        {
            return ListItem();
        }

        return "";
    }

    string ListWeapon()
    {
        string result = "";
        foreach (StatBlock w in GameManager.Instance.weapons)
        {
            result += $"\n[{w.name}] {w.weaponName}";
        }
        return result;
    }

    string ListUpgrade()
    {
        string result = "";
        foreach (Upgrade u in GameManager.Instance.upgradeList)
        {
            result += $"\n[{u.modId}]({u.uCat.ToString()}) {u.modName}: {u.modDesc}";
        }
        return result;
    }

    string ListAbility()
    {
        string result = "";
        foreach (StatBlock a in GameManager.Instance.abilities)
        {
            result += $"\n[{a.name}] {a.abilityName}";
        }
        return result;
    }

    string ListItem()
    {
        string result = "";
        
        return result;
    }

    string SaveGame(string[] args)
    {
        GameManager.Instance.dataPersistenceSystem.SaveGame();
        return "";
    }

    string SaveAndExit(string[] args)
    {
        GameManager.Instance.dataPersistenceSystem.SaveGame();
        Close();
        GameManager.Instance.uiSystem.FadeOut(() =>
        {
            GameManager.Instance.GoToLevel(0);
        });
        return "";
    }

    string LoadGame(string[] args)
    {
        Close();
        GameManager.Instance.uiSystem.FadeOut(() =>
        {
            GameManager.Instance.dataPersistenceSystem.LoadGame();
            GameManager.Instance.GoToLevel(0);
        });
        return "";
    }

    string ResetSave(string[] args)
    {
        Close();
        GameManager.Instance.uiSystem.FadeOut(() =>
        {
            GameManager.Instance.dataPersistenceSystem.LoadGame(true);
            GameManager.Instance.dataPersistenceSystem.SaveGame();
            GameManager.Instance.GoToLevel(0);
        });
        return "";
    }

    string Magnet(string[] args)
    {
        if (args.Length > 1) return "ERROR: Too many parameters";
        Close();
        GameManager.Instance.levelSystem.TriggerAllOrbs();
        return "Triggered magnet";
    }

    string Nuke(string[] args)
    {
        if (args.Length > 1) return "ERROR: Too many parameters";
        Close();
        GameManager.Instance.pawnSystem.Nuke();
        return "Triggered nuke";
    }

    string TriggerWin(string[] args)
    {
        GameManager.Instance.eventSystem.BossDeath();
        return "";
    }

    string TriggerLose(string[] args)
    {
        GameManager.Instance.eventSystem.PlayerDeath();
        return "";
    }

    string Boss(string[] args)
    {
        GameManager.Instance.eventSystem.RequestBossSpawn();
        return "";
    }
}
