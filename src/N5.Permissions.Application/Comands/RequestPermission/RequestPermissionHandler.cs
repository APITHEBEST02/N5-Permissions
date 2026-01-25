using MediatR;
using N5.Permissions.Domain.Repositories;
using N5.Permissions.Domain.Interfaces;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.Comands.RequestPermission;

public class RequestPermissionHandler : IRequestHandler<RequestPermissionCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IElasticsearchService _elasticsearchService;
    private readonly IKafkaProducer _kafkaProducer;

    public RequestPermissionHandler(IUnitOfWork unitOfWork, IElasticsearchService elasticsearchService, IKafkaProducer kafkaProducer)
    {
        _unitOfWork = unitOfWork;
        _elasticsearchService = elasticsearchService;
        _kafkaProducer = kafkaProducer;
    }

    public async Task<int> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
    {
        
        var permission = new Permission
        {
            NombreEmpleado = request.NombreEmpleado,
            ApellidoEmpleado = request.ApellidoEmpleado,
            TipoPermiso = request.TipoPermiso,
            FechaPermiso = request.FechaPermiso
        };

        
        var created = await _unitOfWork.Permissions.AddAsync(permission);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        
        await _elasticsearchService.IndexPermissionAsync(created);

        
        await _kafkaProducer.SendAsync(new OperationMessage
        {
            Id = Guid.NewGuid(),
            NameOperation = "request"
        });

        return created.Id;
    }

}