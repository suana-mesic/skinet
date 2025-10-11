using System;
using Core.Entities;

namespace Core.Interfaces;

//where constraint je ograničenje za tip T
//ovdje T može biti ili BaseEntity ili derivative od BaseEntity (znamo da Product nasljeđuje BaseEntity)
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

    //GetEntityWithSpec<TResult> -> TResult je tip podatka koji ova metoda vraća
    Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec);
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec);
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<bool> SaveAllAsync();
    bool Exists(int id);

}
