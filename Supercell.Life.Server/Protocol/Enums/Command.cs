namespace Supercell.Life.Server.Protocol.Enums
{
    internal enum Command
    {
        // Client
        StartQuest         = 500,
        BuyHero            = 501,
        UpgradeHero        = 502,
        ModifyTeam         = 503,
        BuyResource        = 504,
        StartMatchmake     = 506,
        KickAllianceMember = 510,
        BuyExtra           = 513,
        BuyBooster         = 515,
        ClaimAchievement   = 516,
        CollectMapChest    = 518,
        StartSailing       = 519,
        CollectShipReward  = 520,
        SpeedUpShip        = 521,
        FinishHeroUpgrade  = 522,
        BuyEnergyPackage   = 524,
        UpgradeShip        = 526,
        FinishShipUpgrade  = 527,
        SendAllianceMail   = 528,
        UnlockSpellSlot    = 530,
        UnlockSpell        = 531,
        BrewSpell          = 532,
        StopSpell          = 533,
        FinishSpells       = 534,
        BuyItem            = 535,
        UpgradeItem        = 536,
        AttachItem         = 537,
        SpeedUpItem        = 538,
        MoveCharacter      = 600,
        ReturnToMap        = 602,
        TurnTimedOut       = 605,
        SwapCharacter      = 606,
        Resign             = 609,
        SpeechBubbleReplay = 611,
        AimCharacter       = 612,
        UseSpell           = 613,
        ActionSeen         = 800,

        // Server
        ChangeName              = 1,
        DiamondsAdded           = 2,
        JoinAlliance            = 5,
        LeaveAlliance           = 6,
        ChangeAllianceRole      = 7,
        AllianceSettingsChanged = 9,
        Debug                   = 1000
    }
}
