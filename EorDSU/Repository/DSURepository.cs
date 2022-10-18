using EorDSU.Interface;
using EorDSU.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace EorDSU.Repository
{
    public class DSURepository : IDSURepository
    {
        

        public List<Cathedra> GetCathedra()
        {
            throw new NotImplementedException();
        }

        public List<Department> GetDepartment()
        {
            throw new NotImplementedException();
        }

        public List<Discipline> GetDiscipline()
        {
            throw new NotImplementedException();
        }

        public List<Faculty> GetFaculty()
        {
            throw new NotImplementedException();
        }
    }
}
