﻿using Mediator.PeriodicTable.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RabbitMQ.PeriodicTable.PublisherApp
{
    public class ElementService
    {
        private static readonly Lazy<ElementService> _elementService = new Lazy<ElementService>(() => new ElementService());
        private readonly List<Element> _elenentList = new List<Element>()
        {
            new Element{ ElementId = 1, ShortName = "H", LongName = "Hydrogen"},
            new Element{ ElementId = 2, ShortName = "He", LongName = "Helium"},
            new Element{ ElementId = 3, ShortName = "Li", LongName = "Lithium"},
            new Element{ ElementId = 4, ShortName = "Be", LongName = "Beryllium"},
            new Element{ ElementId = 5, ShortName = "B", LongName = "Boron"},
            new Element{ ElementId = 6, ShortName = "C", LongName = "Carbon"},
            new Element{ ElementId = 7, ShortName = "N", LongName = "Nitrogen"},
            new Element{ ElementId = 8, ShortName = "O", LongName = "Oxygen"},
            new Element{ ElementId = 9, ShortName = "F", LongName = "Fluorine"},
            new Element{ ElementId = 10, ShortName = "Ne", LongName = "Neon"},
            new Element{ ElementId = 11, ShortName = "Na", LongName = "Sodium"},
            new Element{ ElementId = 12, ShortName = "Mg", LongName = "Magnesium"},
            new Element{ ElementId = 13, ShortName = "Al", LongName = "Aluminum"},
            new Element{ ElementId = 14, ShortName = "Si", LongName = "Silicon"},
            new Element{ ElementId = 15, ShortName = "P", LongName = "Phosphorus"},
            new Element{ ElementId = 16, ShortName = "S", LongName = "Sulfur"},
            new Element{ ElementId = 17, ShortName = "Cl", LongName = "Chlorine"},
            new Element{ ElementId = 18, ShortName = "Ar", LongName = "Argon"},
            new Element{ ElementId = 19, ShortName = "K", LongName = "Potassium"},
            new Element{ ElementId = 20, ShortName = "Ca", LongName = "Calcium"},
            new Element{ ElementId = 21, ShortName = "Sc", LongName = "Scandium"},
            new Element{ ElementId = 22, ShortName = "Ti", LongName = "Titanium"},
            new Element{ ElementId = 23, ShortName = "V", LongName = "Vanadium"},
            new Element{ ElementId = 24, ShortName = "Cr", LongName = "Chromium"},
            new Element{ ElementId = 25, ShortName = "Mn", LongName = "Manganese"},
            new Element{ ElementId = 26, ShortName = "Fe", LongName = "Iron"},
            new Element{ ElementId = 27, ShortName = "Co", LongName = "Cobalt"},
            new Element{ ElementId = 28, ShortName = "Ni", LongName = "Nickel"},
            new Element{ ElementId = 29, ShortName = "Cu", LongName = "Copper"},
            new Element{ ElementId = 30, ShortName = "Zn", LongName = "Zinc"},
            new Element{ ElementId = 31, ShortName = "Ga", LongName = "Gallium"},
            new Element{ ElementId = 32, ShortName = "Ge", LongName = "Germanium"},
            new Element{ ElementId = 33, ShortName = "As", LongName = "Arsenic"},
            new Element{ ElementId = 34, ShortName = "Se", LongName = "Selenium"},
            new Element{ ElementId = 35, ShortName = "Br", LongName = "Bromine"},
            new Element{ ElementId = 36, ShortName = "Kr", LongName = "Krypton"},
            new Element{ ElementId = 37, ShortName = "Rb", LongName = "Rubidium"},
            new Element{ ElementId = 38, ShortName = "Sr", LongName = "Strontium"},
            new Element{ ElementId = 39, ShortName = "Y", LongName = "Yttrium"},
            new Element{ ElementId = 40, ShortName = "Zr", LongName = "Zirconium"},
            new Element{ ElementId = 41, ShortName = "Nb", LongName = "Niobium"},
            new Element{ ElementId = 42, ShortName = "Mo", LongName = "Molybdenum"},
            new Element{ ElementId = 43, ShortName = "Tc", LongName = "Technetium"},
            new Element{ ElementId = 44, ShortName = "Ru", LongName = "Ruthenium"},
            new Element{ ElementId = 45, ShortName = "Rh", LongName = "Rhodium"},
            new Element{ ElementId = 46, ShortName = "Pd", LongName = "Palladium"},
            new Element{ ElementId = 47, ShortName = "Ag", LongName = "Silver"},
            new Element{ ElementId = 48, ShortName = "Cd", LongName = "Cadmium"},
            new Element{ ElementId = 49, ShortName = "In", LongName = "Indium"},
            new Element{ ElementId = 50, ShortName = "Sn", LongName = "Tin"},
            new Element{ ElementId = 51, ShortName = "Sb", LongName = "Antimony"},
            new Element{ ElementId = 52, ShortName = "Te", LongName = "Tellurium"},
            new Element{ ElementId = 53, ShortName = "I", LongName = "Iodine"},
            new Element{ ElementId = 54, ShortName = "Xe", LongName = "Xenon"},
            new Element{ ElementId = 55, ShortName = "Cs", LongName = "Cesium"},
            new Element{ ElementId = 56, ShortName = "Ba", LongName = "Barium"},
            new Element{ ElementId = 57, ShortName = "La", LongName = "Lanthanum"},
            new Element{ ElementId = 58, ShortName = "Ce", LongName = "Cerium"},
            new Element{ ElementId = 59, ShortName = "Pr", LongName = "Praseodymium"},
            new Element{ ElementId = 60, ShortName = "Nd", LongName = "Neodymium"},
            new Element{ ElementId = 61, ShortName = "Pm", LongName = "Promethium"},
            new Element{ ElementId = 62, ShortName = "Sm", LongName = "Samarium"},
            new Element{ ElementId = 63, ShortName = "Eu", LongName = "Europium"},
            new Element{ ElementId = 64, ShortName = "Gd", LongName = "Gadolinium"},
            new Element{ ElementId = 65, ShortName = "Tb", LongName = "Terbium"},
            new Element{ ElementId = 66, ShortName = "Dy", LongName = "Dysprosium"},
            new Element{ ElementId = 67, ShortName = "Ho", LongName = "Holmium"},
            new Element{ ElementId = 68, ShortName = "Er", LongName = "Erbium"},
            new Element{ ElementId = 69, ShortName = "Tm", LongName = "Thulium"},
            new Element{ ElementId = 70, ShortName = "Yb", LongName = "Ytterbium"},
            new Element{ ElementId = 71, ShortName = "Lu", LongName = "Lutetium"},
            new Element{ ElementId = 72, ShortName = "Hf", LongName = "Hafnium"},
            new Element{ ElementId = 73, ShortName = "Ta", LongName = "Tantalum"},
            new Element{ ElementId = 74, ShortName = "W", LongName = "Tungsten"},
            new Element{ ElementId = 75, ShortName = "Re", LongName = "Rhenium"},
            new Element{ ElementId = 76, ShortName = "Os", LongName = "Osmium"},
            new Element{ ElementId = 77, ShortName = "Ir", LongName = "Iridium"},
            new Element{ ElementId = 78, ShortName = "Pt", LongName = "Platinum"},
            new Element{ ElementId = 79, ShortName = "Au", LongName = "Gold"},
            new Element{ ElementId = 80, ShortName = "Hg", LongName = "Mercury"},
            new Element{ ElementId = 81, ShortName = "Tl", LongName = "Thallium"},
            new Element{ ElementId = 82, ShortName = "Pb", LongName = "Lead"},
            new Element{ ElementId = 83, ShortName = "Bi", LongName = "Bismuth"},
            new Element{ ElementId = 84, ShortName = "Po", LongName = "Polonium"},
            new Element{ ElementId = 85, ShortName = "At", LongName = "Astatine"},
            new Element{ ElementId = 86, ShortName = "Rn", LongName = "Radon"},
            new Element{ ElementId = 87, ShortName = "Fr", LongName = "Francium"},
            new Element{ ElementId = 88, ShortName = "Ra", LongName = "Radium"},
            new Element{ ElementId = 89, ShortName = "Ac", LongName = "Actinium"},
            new Element{ ElementId = 90, ShortName = "Th", LongName = "Thorium"},
            new Element{ ElementId = 91, ShortName = "Pa", LongName = "Protactinium"},
            new Element{ ElementId = 92, ShortName = "U", LongName = "Uranium"},
            new Element{ ElementId = 93, ShortName = "Np", LongName = "Neptunium"},
            new Element{ ElementId = 94, ShortName = "Pu", LongName = "Plutonium"},
            new Element{ ElementId = 95, ShortName = "Am", LongName = "Americium"},
            new Element{ ElementId = 96, ShortName = "Cm", LongName = "Curium"},
            new Element{ ElementId = 97, ShortName = "Bk", LongName = "Berkelium"},
            new Element{ ElementId = 98, ShortName = "Cf", LongName = "Californium"},
            new Element{ ElementId = 99, ShortName = "Es", LongName = "Einsteinium"},
            new Element{ ElementId = 100, ShortName = "Fm", LongName = "Fermium"},
            new Element{ ElementId = 101, ShortName = "Md", LongName = "Mendelevium"},
            new Element{ ElementId = 102, ShortName = "No", LongName = "Nobelium"},
            new Element{ ElementId = 103, ShortName = "Lr", LongName = "Lawrencium"},
            new Element{ ElementId = 104, ShortName = "Rf", LongName = "Rutherfordium"},
            new Element{ ElementId = 105, ShortName = "Db", LongName = "Dubnium"},
            new Element{ ElementId = 106, ShortName = "Sg", LongName = "Seaborgium"},
            new Element{ ElementId = 107, ShortName = "Bh", LongName = "Bohrium"},
            new Element{ ElementId = 108, ShortName = "Hs", LongName = "Hassium"},
            new Element{ ElementId = 109, ShortName = "Mt", LongName = "Meitnerium"},
            new Element{ ElementId = 110, ShortName = "Ds", LongName = "Darmstadtium"},
            new Element{ ElementId = 111, ShortName = "Rg", LongName = "Roentgenium"},
            new Element{ ElementId = 112, ShortName = "Cn", LongName = "Copernicium"},
            new Element{ ElementId = 113, ShortName = "Nh", LongName = "Nihonium"},
            new Element{ ElementId = 114, ShortName = "Fl", LongName = "Flerovium"},
            new Element{ ElementId = 115, ShortName = "Mc", LongName = "Moscovium"},
            new Element{ ElementId = 116, ShortName = "Lv", LongName = "Livermorium"},
            new Element{ ElementId = 117, ShortName = "Ts", LongName = "Tennessine"},
            new Element{ ElementId = 118, ShortName = "Og", LongName = "Oganesson"}

        };
        public Element GetElementById(int id) => _elenentList.FirstOrDefault(x => x.ElementId == id);
        public Element GetElementByName(string value) => _elenentList.FirstOrDefault(x => x.ShortName == value && x.LongName == value);

        private ElementService() { }

        public static ElementService Instance => _elementService.Value;
    }
}
