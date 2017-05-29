using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models.Repository
{
    public class TagRepository : ITagRepository
    {
        readonly ISessionFactory sessionFactory;
        public TagRepository(ISessionFactory session)
        {
            sessionFactory = session;
        }

        public IEnumerable<Tag> GetAllTags()
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.CreateCriteria<Tag>().List<Tag>();
            }
        }

        public Tag GetTagById(int id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.Get<Tag>(id);
            }
        }

        public Tag GetTagByName(string name)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.CreateCriteria<Tag>().Add(Restrictions.Eq("TagText", name)).UniqueResult<Tag>();
            }
        }

        public int Save(Tag tag)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(tag);
                    transaction.Commit();
                    return tag.Id;
                }
            }
        }

        public IEnumerable<Tag> Save(string[] tags)
        {
            List<Tag> savedTag = new List<Tag>();
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    foreach (string str in tags)
                    {
                        Tag tag = new Tag { TagText = str };
                        session.Save(tag);
                        savedTag.Add(tag);
                    }
                    transaction.Commit();
                }
            }
            return savedTag;
        }
        public IEnumerable<Tag> GatTagsByName(string[] tags)
        {
            List<Tag> loadTags = new List<Tag>();
            using (var session = sessionFactory.OpenSession())
            {
                foreach (string str in tags)
                {
                    Tag tag = session.CreateCriteria<Tag>().Add(Restrictions.Eq("TagText", str)).UniqueResult<Tag>();
                    loadTags.Add(tag);
                }
            }
            return loadTags;
        }
    }
}