﻿using System;

namespace ELTE.Sudoku.Model
{
    /// <summary>
    /// Tárolt játék modellje.
    /// </summary>
    public class StoredGameModel
    {
        /// <summary>
        /// Név lekérdezése, vagy beállítása.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Módosítás idejének lekérdezése, vagy beállítása.
        /// </summary>
        public DateTime Modified { get; set; }
    }
}