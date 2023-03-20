using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBalls.DataBase
{
    public class DataAccess
    {
        public delegate void RefreshListDelegate();
        public static event RefreshListDelegate RefreshList;
        public static List<Agent> GetAgents()
        {
            return BouncingBallsEntities.GetContext().Agents.Where(x => x.IsDeleted == false || x.IsDeleted == null).ToList();
        }
        public static List<Product> GetProducts()
        {
            return BouncingBallsEntities.GetContext().Products.ToList();
        }
        public static void DeleteProductSale(ProductSale product)
        {
            BouncingBallsEntities.GetContext().ProductSales.Remove(product);
            BouncingBallsEntities.GetContext().SaveChanges();
            RefreshList?.Invoke();
        }
        public static void SaveAgent(Agent agent)
        {
            if (agent.ID == 0)
                BouncingBallsEntities.GetContext().Agents.Add(agent);
            BouncingBallsEntities.GetContext().SaveChanges();
            RefreshList?.Invoke();
        }

        public static List<AgentType> GetAgentTypes()
        {
            return BouncingBallsEntities.GetContext().AgentTypes.ToList();
        }

        public static void DeleteAgent(Agent agent)
        {
            AgentPriorityHistory agentPriority = new AgentPriorityHistory();

            //agent.IsDeleted = true;
            BouncingBallsEntities.GetContext().SaveChanges();
            RefreshList?.Invoke();
        }
    }
}
