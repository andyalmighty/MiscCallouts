using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace MiscCallouts.Callouts
{
    [CalloutInfo("Shooting", CalloutProbability.High)]
    public class Shooting : Callout
    {
        //Initialize involved peds
        private Ped balOne;
        private Ped balTwo;
        private Ped balThree;
        private Ped balFour;
        private Ped balFive;
        private Ped famOne;
        private Ped famTwo;
        private Ped famThree;
        private Ped famFour;
        private Ped famFive;

        private Blip Blip1;
        private Blip Blip2;
        private Blip Blip3;
        private Blip Blip4;
        private Blip Blip5;
        private Blip Blip6;
        private Blip Blip7;
        private Blip Blip8;
        private Blip Blip9;
        private Blip Blip10;

        private bool calloutCreated = false;
        private bool fightAgainst = false;

        //Other variables. 
        private Vector3 SpawnPoint;
        private bool OnScene = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(350f));

            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f);
            AddMinimumDistanceCheck(20f, SpawnPoint);

            CalloutMessage = "Gang Activity";
            CalloutPosition = SpawnPoint;

            Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS WE_HAVE CRIME_SHOTS_FIRED IN_OR_ON_POSITION", SpawnPoint); //Plays police scanner audio with callout message.

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            calloutCreated = true;
            
            //Creates peds involved with the shootout.
            balOne = new Ped("csb_ballasog", SpawnPoint, 180f);
            balTwo = new Ped("g_m_y_ballaorig_01", balOne.GetOffsetPosition(new Vector3(1.8f, 0, 0)), 180f);
            balThree = new Ped("g_m_y_ballaeast_01", balTwo.GetOffsetPosition(new Vector3(1.8f, 0, 0)), 180f);
            balFour = new Ped("g_m_y_ballaorig_01", balThree.GetOffsetPosition(new Vector3(1.8f, 0, 0)), 180f);
            balFive = new Ped("g_m_y_ballaorig_01", balFour.GetOffsetPosition(new Vector3(1.8f, 0, 0)), 180f);
            famOne = new Ped("csb_ramp_gang", balOne.GetOffsetPosition(new Vector3(0, 10f, 0)), 360f);
            famTwo = new Ped("g_m_y_famca_01", famOne.GetOffsetPosition(new Vector3(2f, 0, 0)), 360f);
            famThree = new Ped("g_m_y_famdnf_01", famTwo.GetOffsetPosition(new Vector3(2f, 0, 0)), 360f);
            famFour = new Ped("g_m_y_famdnf_01", famThree.GetOffsetPosition(new Vector3(2f, 0, 0)), 360f);
            famFive = new Ped("g_m_y_famca_01", famFour.GetOffsetPosition(new Vector3(2f, 0, 0)), 360f);

            ArmorUp();

            //Creates and attaches blips.
            Blip1 = new Blip(balOne);
            Blip2 = new Blip(balTwo);
            Blip3 = new Blip(balThree);
            Blip4 = new Blip(balFour);
            Blip5 = new Blip(balFive);
            Blip6 = new Blip(famOne);
            Blip7 = new Blip(famTwo);
            Blip8 = new Blip(famThree);
            Blip9 = new Blip(famFour);
            Blip10 = new Blip(famFive);


            //Gives all gang members weapons.
            balOne.Inventory.GiveNewWeapon("WEAPON_PISTOL", 500, true);
            balTwo.Inventory.GiveNewWeapon("WEAPON_MICROSMG", 500, true);
            balThree.Inventory.GiveNewWeapon("WEAPON_PISTOL", 500, true);
            balFour.Inventory.GiveNewWeapon("WEAPON_PISTOL", 500, true);
            balFive.Inventory.GiveNewWeapon("WEAPON_COMBATPISTOL", 500, true);
            famOne.Inventory.GiveNewWeapon("WEAPON_PISTOL", 500, true);
            famTwo.Inventory.GiveNewWeapon("WEAPON_COMBATPISTOL", 500, true);
            famThree.Inventory.GiveNewWeapon("WEAPON_MICROSMG", 500, true);
            famFour.Inventory.GiveNewWeapon("WEAPON_PISTOL", 500, true);
            famFive.Inventory.GiveNewWeapon("WEAPON_PISTOL", 500, true);

            //Displays message on how to end callout and enables route.
            Game.DisplayHelp("Press ~r~End~w~ to end this Callout.", 10000);
            Blip1.EnableRoute(System.Drawing.Color.Red);

            //Play scanner audio.
            Functions.PlayScannerAudio("UNITS_RESPOND_CODE_3");

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (fightAgainst == false) { FightAgainst(); fightAgainst = true; }

            if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 100 && OnScene == false)
            {
                balFive.Tasks.FireWeaponAt(famTwo, 1000, FiringPattern.BurstFirePistol);
                famTwo.Tasks.FireWeaponAt(balFive, 1000, FiringPattern.BurstFirePistol);
                OnScene = true;
            }

            if (Game.IsKeyDownRightNow(System.Windows.Forms.Keys.End))
            {
                End();
            }

            if (balOne.IsDead && balTwo.IsDead && balThree.IsDead && balFour.IsDead && balFive.IsDead && famOne.IsDead && famTwo.IsDead && famThree.IsDead && famFour.IsDead && famFive.IsDead) { End(); }

            DeleteBlip(balOne, Blip1);
            DeleteBlip(balTwo, Blip2);
            DeleteBlip(balThree, Blip3);
            DeleteBlip(balFour, Blip4);
            DeleteBlip(balFive, Blip5);
            DeleteBlip(famOne, Blip6);
            DeleteBlip(famTwo, Blip7);
            DeleteBlip(famThree, Blip8);
            DeleteBlip(famFour, Blip9);
            DeleteBlip(famFive, Blip10);

            base.Process();
        }

        public override void End()
        {
            //Dismisses peds from plugins memory.
            if (balOne.Exists()) { balOne.Dismiss(); }
            if (balTwo.Exists()) { balTwo.Dismiss(); }
            if (balThree.Exists()) { balThree.Dismiss(); }
            if (balFour.Exists()) { balFour.Dismiss(); }
            if (balFive.Exists()) { balFive.Dismiss(); }
            if (famOne.Exists()) { famOne.Dismiss(); }
            if (famTwo.Exists()) { famTwo.Dismiss(); }
            if (famThree.Exists()) { famThree.Dismiss(); }
            if (famFour.Exists()) { famFour.Dismiss(); }
            if (famFive.Exists()) { famFive.Dismiss(); }

            //Deletes blips.
            if (Blip1.Exists()) { Blip1.Delete(); }
            if (Blip2.Exists()) { Blip2.Delete(); }
            if (Blip3.Exists()) { Blip3.Delete(); }
            if (Blip4.Exists()) { Blip4.Delete(); }
            if (Blip5.Exists()) { Blip5.Delete(); }
            if (Blip6.Exists()) { Blip6.Delete(); }
            if (Blip7.Exists()) { Blip7.Delete(); }
            if (Blip8.Exists()) { Blip8.Delete(); }
            if (Blip9.Exists()) { Blip9.Delete(); }
            if (Blip10.Exists()) { Blip10.Delete(); }



            //Displays Code 4 message & audio.
            if (calloutCreated == true)
            {
                Game.DisplayNotification("~b~Dispatch:~w~ Attention all units, the ~r~Gang Shooting~w~ call is ~g~Code 4.~w~");
                Functions.PlayScannerAudio("ATTENTION_ALL_UNITS WE_ARE_CODE_4");
            }

            base.End();
        }

        private void DeleteBlip(Entity e, Blip b)
        {
            if (b.Exists() && (!e.Exists() || e.IsDead))
            {
                b.Delete();
            }
        }

        private void ArmorUp()
        {
            balOne.Armor = 100;
            balTwo.Armor = 100;
            balThree.Armor = 100;
            balFour.Armor = 100;
            balFive.Armor = 100;

            famOne.Armor = 100;
            famTwo.Armor = 100;
            famThree.Armor = 100;
            famFour.Armor = 100;
            famFive.Armor = 100;

        }

        private void FightAgainst()
        {
            if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) < 50)
            {
                if (balOne.IsAlive) { balOne.Tasks.FightAgainst(Game.LocalPlayer.Character); }
                if (balTwo.IsAlive) { balTwo.Tasks.FightAgainst(Game.LocalPlayer.Character); }
                if (balThree.IsAlive) { balThree.Tasks.FightAgainst(Game.LocalPlayer.Character); }
                if (balFour.IsAlive) { balFour.Tasks.FightAgainst(Game.LocalPlayer.Character); }
                if (balFive.IsAlive) { balFive.Tasks.FightAgainst(Game.LocalPlayer.Character); }

                if (famOne.IsAlive) { famOne.Tasks.FightAgainst(Game.LocalPlayer.Character); }
                if (famTwo.IsAlive) { famTwo.Tasks.FightAgainst(Game.LocalPlayer.Character); }
                if (famThree.IsAlive) { famThree.Tasks.FightAgainst(Game.LocalPlayer.Character); }
                if (famFour.IsAlive) { famFour.Tasks.FightAgainst(Game.LocalPlayer.Character); }
                if (famFive.IsAlive) { famFive.Tasks.FightAgainst(Game.LocalPlayer.Character); }
            }
        }
    }
}
