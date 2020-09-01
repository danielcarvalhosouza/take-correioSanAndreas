using System;
using System.Collections.Generic;
using System.Text;
using Take.CorreioSanAndreas.Domain.Entities;

namespace Take.CorreioSanAndreas.Domain.Interfaces
{
    public interface IShortestPathFinderService
    {
        Node[] FindShortestPath(Node @from, Node to);
    }
}
