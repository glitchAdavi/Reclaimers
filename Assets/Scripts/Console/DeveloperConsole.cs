using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DeveloperConsole : MonoBehaviour
{

    public GameObject console;
    public bool consoleEnabled = false;

    public TMP_InputField consoleInput;
    public TMP_Text consoleText;

    public delegate string Command(string[] args);

    private Dictionary<string, Command> _consoleLogic = new Dictionary<string, Command>();
    private Dictionary<string, string> _consoleHelp = new Dictionary<string, string>();


    private void OnEnable()
    {
        console = Instantiate(GameManager.current.gameInfo.devConsolePrefab, GameObject.Find("Canvas").transform);
        consoleInput = console.GetComponent<ConsoleHelper>().consoleInput;
        consoleText = console.GetComponent<ConsoleHelper>().consoleText;

        console.SetActive(consoleEnabled);

        AddCommand("help", Help, "help [optional] || Show info on all commands or only [optional] command info");
        AddCommand("cls", Cls, "cls || Clear screen");
        AddCommand("close", Close, "close || Close the Console");
        AddCommand("give", Give, "Multiple variations:" +
            "\ngive xp [x] || Give player [x] xp" +
            "\ngive level [x] [skipUpgrade=false] || Give player [x] levels, [skipUpgrade] skips the upgrade selection menu and no upgrade is given" +
            "\ngive upgrade [x] || Give player [x] upgrade" +
            "\ngive weapon [x] || Give player [x] weapon" +
            "\ngive ability [x] || Give player [x] ability");
        AddCommand("list", List, "Multiple variations:" +
            "\nlist weapon || List all weapons" +
            "\nlist upgrade || List all upgrades" +
            "\nlist ability || List all abilities");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            consoleEnabled = !consoleEnabled;
            console.SetActive(consoleEnabled);
            GameManager.current.eventService.RequestEnableControlAll(!consoleEnabled);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (consoleInput.text != null)
            {
                Process(consoleInput.text);
                consoleInput.text = "";
                consoleInput.ActivateInputField();
            }
        }
    }

    public void AddCommand(string key, Command commandEvent, string explain)
    {
        if (!_consoleLogic.ContainsKey(key)) _consoleLogic.Add(key, commandEvent);
        else _consoleLogic[key] += commandEvent;

        if (!_consoleHelp.ContainsKey(key)) _consoleHelp.Add(key, explain);
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
            if (!commandResult.Equals("\n> ")) consoleText.text += commandResult;
        }
        else consoleText.text += "\n> " + "ERROR: command not found";
    }

    string Cls(string[] args)
    {
        if (args.Length > 0) return "ERROR: Wrong parameters";

        consoleText.text = "> For all commands type 'help'";
        return "";
    }

    string Help(string[] args)
    {
        if (args.Length > 1) return "ERROR: Wrong parameters";

        if (args.Length > 0)
        {
            if (_consoleHelp.ContainsKey(args[0]))
            {
                consoleText.text += "\n> -------------------------";
                consoleText.text += $"\n> {_consoleHelp[args[0]]}";
                consoleText.text += "\n> -------------------------";
            }
            
        } else
        {
            consoleText.text += "\n> -------------------------";

            foreach (var c in _consoleLogic)
                consoleText.text += $"\n> {_consoleHelp[c.Key]}";

            consoleText.text += "\n> -------------------------";
        }            
        return "";
    }

    string Close(string[] args)
    {
        if (args.Length > 0) return "ERROR: Wrong parameters";

        consoleEnabled = !consoleEnabled;
        console.SetActive(consoleEnabled);
        GameManager.current.eventService.RequestEnableControlAll(!consoleEnabled);
        return "";
    }

    string Give(string[] args)
    {
        if (args.Length < 1) return "ERROR: Wrong parameters";

        if (args[0].Equals("xp"))
        {
            return GiveXp(args.Skip(1).ToArray());
        }

        if (args[0].Equals("level"))
        {
            return GiveLevel(args.Skip(1).ToArray());
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

        return "";
    }

    string GiveXp(string[] args)
    {
        if (args.Length < 1) return "ERROR: Quantity needed";
        if (args.Length > 1) return "ERROR: Wrong parameters";

        float result;
        if (float.TryParse(args[0], out result))
        {
            GameManager.current.eventService.GivePlayerXp(result);
            return $"{result} xp given to Player";
        } else
        {
            return "ERROR: give xp [x] || x has to be a number";
        }
    }

    string GiveLevel(string[] args)
    {
        if (args.Length < 1) return "ERROR: Quantity needed";
        if (args.Length > 2) return "ERROR: Wrong parameters";

        int result;
        if (int.TryParse(args[0], out result))
        {
            bool skipLevel = false;
            if (args.Length > 1 && bool.TryParse(args[1], out skipLevel)) GameManager.current.eventService.GivePlayerLevel(result, skipLevel);
            else GameManager.current.eventService.GivePlayerLevel(result);
            return $"{result} levels given to Player";
        }
        else
        {
            return "ERROR: give level [x] || x has to be a number";
        }
    }

    string GiveWeapon(string[] args)
    {
        return "ERROR: Not implemented yet";
    }

    string GiveUpgrade(string[] args)
    {
        return "ERROR: Not implemented yet";
    }

    string GiveAbility(string[] args)
    {
        return "ERROR: Not implemented yet";
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

        return "";
    }

    string ListWeapon()
    {
        string result = "";
        foreach (WeaponStatBlock w in GameManager.current.allWeapons)
        {
            result += $"\n[{w.internalName}] {w.weaponName}: {w.weaponDescription}";
        }
        return result;
    }

    string ListUpgrade()
    {
        string result = "";
        foreach (PawnUpgrade pu in GameManager.current.allPawnUpgrades)
        {
            result += $"\n[{pu.internalName}](pawn) {pu.upgradeName}: {pu.upgradeDesc}";
        }
        foreach (WeaponUpgrade wu in GameManager.current.allWeaponUpgrades)
        {
            result += $"\n[{wu.internalName}](weapon) {wu.upgradeName}: {wu.upgradeDesc}";
        }
        foreach (AbilityUpgrade au in GameManager.current.allAbilityUpgrades)
        {
            result += $"\n[{au.internalName}](ability) {au.upgradeName}: {au.upgradeDesc}";
        }
        return result;
    }

    string ListAbility()
    {
        string result = "";
        foreach (AbilityStatBlock a in GameManager.current.allAbilities)
        {
            result += $"\n[{a.internalName}] {a.abilityName}: {a.abilityDescription}";
        }
        return result;
    }
}
