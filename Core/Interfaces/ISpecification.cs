using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T>
{
    //ovo je za filtriranje
    Expression<Func<T, bool>>? Criteria { get; }

    //ovo je za sortiranje
    //object će biti ili decimal (ako se radi sortiranje po vrijednostima cijene npr) ili string (ako se radi sortiranje po imenu)
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }

    bool IsDistinct { get; }


}

//ovo vraća TResult
public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T, TResult>>? Select { get; }
}