using System;
using System.IO.Pipelines;
using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specifications;

public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{
    //ispod je konstruktor koji nam omogucava da kreiramo specifikaciju bez da 
    //navedemo where klauzulu (odnosno kriterij)
    //this (null) automatski poziva primary construktor i prosljeđuje null -> Expression<Func<T, bool>>? criteria=null
    //zbog toga moramo staviti da je Criteria property optional, a onda i u interfejsu mora biti tako definisan


    //Expression je recimo kada napišemo ovo query.Where(x=>x.Brand==brand) 
    //zelimo sve proizvode gdje je brend jednak brendu kojeg trazimo

    protected BaseSpecification() : this(null) { }
    public Expression<Func<T, bool>>? Criteria => criteria; //ovo je property koji ima samo getter
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    public bool IsDistinct { get; private set; }

    protected void ApplyDistinct()
    {
        IsDistinct = true;
    }

    //metoda za setovanje orderBy-a, moze se pozivati iz ove i izvedenih klasa (ProductController je izvedena)
    //AddOrderBy koristi se za asc i sortiranje po Name-u
    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }
    //AddOrderBy koristi se za desc
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }
}

public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria)
: BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
    protected BaseSpecification() : this(null) { }
    public Expression<Func<T, TResult>>? Select { get; private set; }

    protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}