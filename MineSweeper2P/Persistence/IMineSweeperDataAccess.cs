using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MineSweeper2Pt8hgxr.Model;

namespace MineSweeper2Pt8hgxr.Persistence
{
    public interface IMineSweeperDataAccess
    {
        Task<MineSweeper2PGameState> LoadAsync(String path);

        Task SaveAsync(String path, MineSweeper2PGameState gameState);
    }
}
