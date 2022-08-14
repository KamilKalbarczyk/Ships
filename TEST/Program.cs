using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TEST
{
    class Program
    {
        public static void Main()
        {
            var game = new Game();

            game.SetShips(new int[] { 5, 4, 4 });
            game.DisplayBoard();
            game.Play();
        }
    }
}