1:use Loggerfactor instead of SQL profiler
using Microsoft.Extensions.Logging;
nuget>  Microsoft.Extensions.Logging.Console

2:understand EFCore sql script
https://www.brentozar.com/archive/2017/05/case-entity-framework-cores-odd-sql/

3:
## ADD
DBSet:
_context.Samurais.Add(samurai);
_context.Samurais.AddRange(samuraiList);
DBContext:
_context.Add(samurai);
_context.AddRange(samurai,battle);
## UPDATE
_context.Samurais.Update(samurai);
_context.Samurais.UpdateRange(samuraiList);

_context.Update(samurai);
_context.UpdateRange(samurai,battle);
## DELETE
_context.Samurais.Remove(samurai);
_context.Samurais.RemoveRange(samuraiList);

_context.Remove(samurai);
_context.RemoveRange(samurai,battle);
