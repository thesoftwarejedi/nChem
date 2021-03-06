using System;
using System.Linq;

namespace nChem
{
    public sealed class Subshell
    {
        /// <summary>
        /// Initializes an instance of the <see cref="Subshell"/> class.
        /// </summary>
        /// <param name="parent">The parent shell.</param>
        /// <param name="label">The label.</param>
        /// <param name="capacity">The electron capacity.</param>
        internal Subshell(char parent, char label, int capacity)
        {
            Parent = parent;
            Label = label;
            Orbitals = new Orbital[capacity / 2];

            int[] range = (-AzimuthalQuantumNumber).To(AzimuthalQuantumNumber).ToArray();

            for (var x = 0; x < Orbitals.Length; x++)
                Orbitals[x] = new Orbital(range[x]);
        }

        /// <summary>
        /// Gets the label of the <see cref="Subshell"/>.
        /// </summary>
        public char Label { get; }

        /// <summary>
        /// Gets the parent shell of the <see cref="Subshell"/>.
        /// </summary>
        public char Parent { get; internal set; }

        /// <summary>
        /// Gets the orbitals of the <see cref="Subshell"/>.
        /// </summary>
        public Orbital[] Orbitals { get; }

        /// <summary>
        /// Gets the azimuthal quantum number, which represents the zero-based index of the <see cref="Subshell"/>.
        /// </summary>
        public int AzimuthalQuantumNumber => ChemistryUtils.GetSubshellIndex(Label);

        /// <summary>
        /// Gets the electron capacity in the <see cref="Subshell"/>.
        /// </summary>
        public int Capacity => Orbitals.Length*2;

        /// <summary>
        /// Gets the amount of paired electrons in the <see cref="Subshell"/>.
        /// </summary>
        public int PairedElectrons => Electrons - UnpairedElectrons;

        /// <summary>
        /// Gets the amount of unpaired electrons in the <see cref="Subshell"/>.
        /// </summary>
        public int UnpairedElectrons => Orbitals.Count(x => !x.IsPaired());

        /// <summary>
        /// Gets the amount of electrons in the <see cref="Subshell"/>.
        /// </summary>
        public int Electrons => Orbitals.Sum(x => x.GetElectrons().Where(y => y != null).ToArray().Length);

        /// <summary>
        /// Gets an orbital with a specific zero-based index.
        /// </summary>
        /// <param name="index">The zero-based index of an orbital.</param>
        /// <returns></returns>
        public Orbital this[int index] => Orbitals[index];

        /// <summary>
        /// Populates the <see cref="Subshell"/> orbitals with <c>n</c> electrons.
        /// </summary>
        /// <param name="n">The amount of electrons to populate with.</param>
        public void Populate(int n)
        {
            if (n > Capacity || n < 1)
                throw new IndexOutOfRangeException(nameof(n));

            int remainingElectrons = n;

            foreach (var orbital in Orbitals)
            {
                if (remainingElectrons <= 0)
                    break;

                if (Orbitals.Length == 1)
                {
                    orbital.Populate();
                    orbital.Populate();

                    remainingElectrons -= 2;
                    continue;
                }

                orbital.Populate();
                remainingElectrons--;
            }

            if (Orbitals.Length <= 1) return;

            foreach (var orbital in Orbitals)
            {
                if (remainingElectrons == 0)
                    break;

                orbital.Populate();
                remainingElectrons--;
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{ChemistryUtils.GetShellIndex(Parent) + 1}{Label}";
        }
    }
}