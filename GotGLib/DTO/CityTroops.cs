using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DTO
{
    public class CityTroops: BaseDTO
    {
        private int m_Guard;

        public int Guard
        {
            get { return m_Guard; }
            set { SetProperty(ref m_Guard, value); }
        }
        private int m_Balista;

        public int Balista
        {
            get { return m_Balista; }
            set { SetProperty(ref m_Balista, value); }
        }

        private int m_Ranger;

        public int Ranger
        {
            get { return m_Ranger; }
            set { SetProperty(ref m_Ranger, value); }
        }

        private int m_Triari;

        public int Triari
        {
            get { return m_Triari; }
            set { SetProperty(ref m_Triari, value); }
        }

        private int m_Priestess;

        public int Priestess
        {
            get { return m_Priestess; }
            set { SetProperty(ref m_Priestess, value); }
        }

        private int m_Vanquisher;

        public int Vanquisher
        {
            get { return m_Vanquisher; }
            set { SetProperty(ref m_Vanquisher, value); }
        }

        private int m_Sorcerer;

        public int Sorcerer
        {
            get { return m_Sorcerer; }
            set { SetProperty(ref m_Sorcerer, value); }
        }

        private int m_Scout;

        public int Scout
        {
            get { return m_Scout; }
            set { SetProperty(ref m_Scout, value); }
        }

        private int m_Arbalist;

        public int Arbalist
        {
            get { return m_Arbalist; }
            set { SetProperty(ref m_Arbalist, value); }
        }

        private int m_Praetor;

        public int Praetor
        {
            get { return m_Praetor; }
            set { SetProperty(ref m_Praetor, value); }
        }

        private int m_Horseman;

        public int Horseman
        {
            get { return m_Horseman; }
            set { SetProperty(ref m_Horseman, value); }
        }

        private int m_Druid;

        public int Druid
        {
            get { return m_Druid; }
            set { SetProperty(ref m_Druid, value); }
        }

        private int m_Ram;

        public int Ram
        {
            get { return m_Ram; }
            set { SetProperty(ref m_Ram, value); }
        }

        private int m_Scorpion;

        public int Scorpion
        {
            get { return m_Scorpion; }
            set { SetProperty(ref m_Scorpion, value); }
        }

        private int m_Galley;

        public int Galley
        {
            get { return m_Galley; }
            set { SetProperty(ref m_Galley, value); }
        }

        private int m_Stinger;

        public int Stinger
        {
            get { return m_Stinger; }
            set { SetProperty(ref m_Stinger, value); }
        }

        private int m_Warship;

        public int Warship
        {
            get { return m_Warship; }
            set { SetProperty(ref m_Warship, value); }
        }

        private int m_Senator;

        public int Senator
        {
            get { return m_Senator; }
            set { SetProperty(ref m_Senator, value); }
        }

        private int m_TotalDefTS;

        public int TotalDefTS
        {
            get { return m_TotalDefTS; }
            set { SetProperty(ref m_TotalDefTS, value); }
        }

        private int m_TotalOffTS;

        public int TotalOffTS
        {
            get { return m_TotalOffTS; }
            set { SetProperty(ref m_TotalOffTS, value); }
        }

        private int m_TotalTS;

        public int TotalTS
        {
            get { return m_TotalTS; }
            set { SetProperty(ref m_TotalTS, value); }
        }

        public void RecalcFields()
        {
            TotalDefTS = Guard + Balista*10 + Ranger + Triari + Priestess + Arbalist*2 + Praetor*2 + Stinger*2;
            TotalOffTS = Vanquisher + Horseman*2 + Sorcerer + Druid*2 + Ram*10 + Scorpion*10 + Galley*100 + Warship*400;
            TotalTS = TotalDefTS + TotalOffTS + Scout + Senator;
        }

        public void Add(CityTroops t)
        {
            Guard += t.Guard;
            Balista += t.Balista;
            Ranger += t.Ranger;
            Triari += t.Triari;
            Priestess += t.Priestess;
            Arbalist += t.Arbalist;
            Praetor += t.Praetor;
            Stinger += t.Stinger;
            Vanquisher += t.Vanquisher;
            Horseman += t.Horseman;
            Sorcerer += t.Sorcerer;
            Druid += t.Druid;
            Ram += t.Ram;
            Scorpion += t.Scorpion;
            Galley += t.Galley;
            Warship += t.Warship;
            Scout += t.Scout;
            Senator += t.Senator;
        }
    }
}
