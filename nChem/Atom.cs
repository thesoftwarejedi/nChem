﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace nChem
{
    /// <summary>
    /// Represents an atom of an <see cref="Element"/>.
    /// </summary>
    public sealed class Atom
    {
        /// <summary>
        /// Initializes an instance of the <see cref="Atom"/> class.
        /// </summary>
        /// <param name="element">The element of the atom.</param>
        public Atom(Element element)
        {
            Element = element;
            Electrons = Element.Electrons;
            Neutrons = Element.Neutrons;
            Protons = Element.Protons;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Atom"/> class.
        /// </summary>
        /// <param name="atomicNumber">The atomic number of the element.</param>
        public Atom(int atomicNumber)
            : this(new Element(atomicNumber))
        { }

        /// <summary>
        /// Gets or sets the amount of electrons within the <see cref="Atom"/>.
        /// </summary>
        public int Electrons { get; set; }

        /// <summary>
        /// Gets or sets the amount of neutrons within the <see cref="Atom"/>.
        /// </summary>
        public int Neutrons { get; set; }

        /// <summary>
        /// Gets or sets the amount of neutrons within the <see cref="Atom"/>.
        /// </summary>
        public int Protons { get; set; }

        /// <summary>
        /// Gets the element of the <see cref="Atom"/>.
        /// </summary>
        public Element Element { get; }

        /// <summary>
        /// Gets the magnetism of the <see cref="Atom"/>.
        /// </summary>
        public Magnetism Magnetism
            => GetShellConfiguration().UnpairedElectrons > 0 ? Magnetism.Paramagnetic : Magnetism.Diamagnetic;

        /// <summary>
        /// Returns the shells of the <see cref="Element"/>.
        /// </summary>
        /// <returns></returns>
        public ShellConfiguration GetShellConfiguration()
        {
            var shells = new Dictionary<char, int>();
            int shellCount = ChemistryUtils.GetShellCount(Element.Electrons);

            int electrons = Element.Electrons;

            for (var i = 0; i < shellCount; i++)
            {
                if (electrons == 0)
                    break;

                int capacity = ChemistryUtils.GetShellCapacity(i);
                char shell = ChemistryUtils.ShellLabels[i];

                if (!shells.ContainsKey(shell))
                    shells.Add(shell, 0);

                if (electrons > capacity)
                {
                    shells[shell] = capacity;
                    electrons -= capacity;

                    continue;
                }

                shells[shell] = electrons;
                electrons = 0;
            }

            return new ShellConfiguration(shells);
        }

        /// <summary>
        /// Determines whether the current <see cref="Atom"/> is an alkali metal.
        /// </summary>
        /// <returns></returns>
        public bool IsAlkaliMetal()
        {
            return GetShellConfiguration().GetValenceShell().Electrons == 1;
        }

        /// <summary>
        /// Determines whether the current <see cref="Atom"/> is a halogen.
        /// </summary>
        /// <returns></returns>
        public bool IsHalogen()
        {
            return GetShellConfiguration().GetValenceShell().Electrons ==
                   GetShellConfiguration().GetValenceShell().Capacity - 1;
        }
        
        /// <summary>
        /// Determines whether the <see cref="Atom"/> is an ion.
        /// </summary>
        /// <returns></returns>
        public bool IsIon()
        {
            return Electrons != Protons;
        }

        /// <summary>
        /// Converts the current <see cref="Atom"/> instance to an <see cref="Ion"/>.
        /// </summary>
        /// <returns></returns>
        public Ion ToIon()
        {
            if (!IsIon())
                throw new Exception("The current atom isn't an ion.");

            return new Ion(this);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Element.Symbol;
        }

        /// <summary>
        /// Determines whether the left <see cref="Atom"/> instance is equal to the right <see cref="Atom"/> instance.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns></returns>
        public static bool operator ==(Atom left, Atom right)
        {
            if (left == null && right == null)
                return true;

            if (left == null || right == null)
                return false;

            return left.Electrons == right.Electrons &&
                   left.Protons == right.Protons &&
                   left.Neutrons == right.Neutrons;
        }

        /// <summary>
        /// Determines whether the left <see cref="Atom"/> instance is not equal to the right <see cref="Atom"/>.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns></returns>
        public static bool operator !=(Atom left, Atom right)
        {
            return !(left == right);
        }
    }
}