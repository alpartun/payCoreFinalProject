using AutoMapper;
using NHibernate;
using PayCoreFinalProject.Base.Response;
using PayCoreFinalProject.Data.Repository;
using PayCoreFinalProject.Service.Base.Abstract;
using Serilog;

namespace PayCoreFinalProject.Service.Base.Concrete;

public abstract class BaseService<Dto, Entity> : IBaseService<Dto, Entity> where Entity : class
{
    protected readonly ISession _session;
    protected readonly IMapper _mapper;
    protected readonly IHibernateRepository<Entity> _hibernateRepository;

    public BaseService(ISession session, IMapper mapper) : base()
    {
        _session = session;
        _mapper = mapper;
        _hibernateRepository = new HibernateRepository<Entity>(session);
    }

    public virtual BaseResponse<Dto> GetById(int id)
    {
        try
        {
            var tempEntity = _hibernateRepository.GetById(id);
            var result = _mapper.Map<Entity, Dto>(tempEntity);
            if (result == null)
            {
                return new BaseResponse<Dto>($"{typeof(Entity)} not found.");
            }

            return new BaseResponse<Dto>(result);
        }
        catch (Exception e)
        {
            Log.Error("BaseService.GetById", e);
            return new BaseResponse<Dto>(e.Message);
        }
    }

    public BaseResponse<IEnumerable<Dto>> GetAll()
    {
        try
        {
            var tempEntity = _hibernateRepository.Entities.ToList();
            var result = _mapper.Map<IEnumerable<Entity>, IEnumerable<Dto>>(tempEntity);
            return new BaseResponse<IEnumerable<Dto>>(result);
        }
        catch (Exception e)
        {
            Log.Error("BaseService.GetAll", e);
            return new BaseResponse<IEnumerable<Dto>>(e.Message);
        }
    }

    public BaseResponse<Dto> Insert(Dto insertResource)
    {
        try
        {
            var tempEntity = _mapper.Map<Dto, Entity>(insertResource);

            _hibernateRepository.BeginTransaction();
            _hibernateRepository.Save(tempEntity);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();
            var result = _mapper.Map<Entity, Dto>(tempEntity);
            return new BaseResponse<Dto>(result);
        }
        catch (Exception e)
        {
            Log.Error("BaseService.Insert", e);
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<Dto>(e.Message);
        }
    }

    public BaseResponse<Dto> Update(int id, Dto updateResource)
    {
        var tempEntity = _hibernateRepository.GetById(id);
        if (tempEntity is null)
        {
            return new BaseResponse<Dto>("Record is not found.");
        }

        var entity = _mapper.Map<Dto, Entity>(updateResource, tempEntity);
        try
        {
            _hibernateRepository.BeginTransaction();
            _hibernateRepository.Update(entity);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();
            var resource = _mapper.Map<Entity, Dto>(entity);
            return new BaseResponse<Dto>(resource);
        }
        catch (Exception e)
        {
            Log.Error("BaseService.Update", e);

            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<Dto>(e.Message);
        }
    }

    public BaseResponse<Dto> Remove(int id)
    {
        try
        {
            var tempEntity = _hibernateRepository.GetById(id);
            if (tempEntity is null)
            {
                new BaseResponse<Dto>("Record is not found.");
            }

            _hibernateRepository.BeginTransaction();
            _hibernateRepository.Delete(id);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();

            var result = _mapper.Map<Entity, Dto>(tempEntity);

            return new BaseResponse<Dto>(result);
        }
        catch (Exception e)
        {
            Log.Error("BaseService.Remove", e);

            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<Dto>(e.Message);
        }
    }
}