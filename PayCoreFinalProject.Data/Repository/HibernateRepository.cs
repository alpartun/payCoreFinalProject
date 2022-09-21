using System.Linq.Expressions;
using NHibernate;

namespace PayCoreFinalProject.Data.Repository;

public class HibernateRepository<Entity> : IHibernateRepository<Entity> where Entity : class
{
    private readonly ISession _session;
    private ITransaction _transaction;

    public HibernateRepository(ISession session)
    {
        _session = session;
    }

    public void BeginTransaction()
    {
        _transaction = _session.BeginTransaction();
    }

    public void Commit()
    {
        _transaction.Commit();
    }

    public void Rollback()
    {
        _transaction.Rollback();
    }

    public void CloseTransaction()
    {
        _transaction.Dispose();
    }

    public void Save(Entity entity)
    {
        _session.Save(entity);
    }

    public void Update(Entity entity)
    {
        _session.Update(entity);
    }

    public void Delete(int id)
    {
        var entity = GetById(id);
        if (entity != null)
        {
            _session.Delete(entity);
        }
    }

    public List<Entity> GetAll()
    {
        return _session.Query<Entity>().ToList();
    }

    public Entity GetById(int id)
    {
        var entity = _session.Get<Entity>(id);
        return entity;
    }

    public IEnumerable<Entity> Find(Expression<Func<Entity, bool>> expression)
    {
        return _session.Query<Entity>().Where(expression).ToList();
    }

    public IEnumerable<Entity> Where(Expression<Func<Entity, bool>> where)
    {
        return _session.Query<Entity>().Where(where).AsQueryable();
    }

    public IQueryable<Entity> Entities => _session.Query<Entity>();
}