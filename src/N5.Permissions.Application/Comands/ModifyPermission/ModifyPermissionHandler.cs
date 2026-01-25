using MediatR;
using N5.Permissions.Domain.Repositories;
using N5.Permissions.Domain.Interfaces;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Application.Exceptions;

namespace N5.Permissions.Application.Comands.ModifyPermission;

public class ModifyPermissionHandler: IRequestHandler<ModifyPermissionCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IElasticsearchService _elasticsearchService;  
    private readonly IKafkaProducer _kafkaProducer;

    public ModifyPermissionHandler(IUnitOfWork unitOfWork, IElasticsearchService elasticsearchService, IKafkaProducer kafkaProducer)
    {
        _unitOfWork = unitOfWork;
        _elasticsearchService = elasticsearchService;
        _kafkaProducer = kafkaProducer;
    }

    public async Task<int> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await _unitOfWork.Permissions.GetByIdAsync(request.Id);
        if (permission == null)
        {
            throw new NotFoundException("Permission not found");
        }

        permission.NombreEmpleado = request.NombreEmpleado;
        permission.ApellidoEmpleado = request.ApellidoEmpleado;
        permission.TipoPermiso = request.TipoPermiso;
        permission.FechaPermiso = request.FechaPermiso;

        await _unitOfWork.Permissions.UpdateAsync(permission);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _elasticsearchService.IndexPermissionAsync(permission);

        await _kafkaProducer.SendAsync(new OperationMessage
        {
            Id = Guid.NewGuid(),
            NameOperation = "modify"
        });

        return permission.Id;
    } 
}