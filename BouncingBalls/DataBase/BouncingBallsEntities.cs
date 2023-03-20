using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBalls.DataBase
{
    public partial class BouncingBallsEntities
    {
        private static BouncingBallsEntities context;
        public static BouncingBallsEntities GetContext()
        {
            if (context == null)
                context = new BouncingBallsEntities();
            return context;
        }
    }
}
