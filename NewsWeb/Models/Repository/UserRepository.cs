using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models.Repository
{
    public class UserRepository : IUserRepository
    {
        readonly ISessionFactory sessionFactory;

        public UserRepository(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public int Save(AppUser u)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(u);
                    transaction.Commit();
                    return u.Id;
                }
            }
        }
        public AppUser GetById(int Id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.Get<AppUser>(Id);
            }
        }
        public AppUser FindByName(string name)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var user = session.CreateCriteria<AppUser>()
                    .Add(Restrictions.Eq("UserName", name))
                    .UniqueResult<AppUser>();
                return user;

            }
        }

       

    }
}