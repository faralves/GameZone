using System;
using System.Collections.Generic;
using GameZone.Core.Messages;

namespace GameZone.Core.DomainObjects
{
    public abstract class EntityForIdAspnetUsers
    {
        public Guid IdAspNetUsers { get; set; }

        protected EntityForIdAspnetUsers()
        {
            IdAspNetUsers = Guid.NewGuid();
        }

        private List<Event> _notificacoes;
        public IReadOnlyCollection<Event> Notificacoes => _notificacoes?.AsReadOnly();

        public void AdicionarEvento(Event evento)
        {
            _notificacoes = _notificacoes ?? new List<Event>();
            _notificacoes.Add(evento);
        }

        public void RemoverEvento(Event eventItem)
        {
            _notificacoes?.Remove(eventItem);
        }

        public void LimparEventos()
        {
            _notificacoes?.Clear();
        }

        #region Comparações

        public override bool Equals(object obj)
        {
            var compareTo = obj as EntityForIdAspnetUsers;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return IdAspNetUsers.Equals(compareTo.IdAspNetUsers);
        }

        public static bool operator ==(EntityForIdAspnetUsers a, EntityForIdAspnetUsers b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EntityForIdAspnetUsers a, EntityForIdAspnetUsers b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + IdAspNetUsers.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={IdAspNetUsers}]";
        }

        #endregion
    }
}