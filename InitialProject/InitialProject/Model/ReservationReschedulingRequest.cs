﻿using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;

namespace InitialProject.Model
{
    public class ReservationReschedulingRequest : ISerializable
    {
        public int Id { get; set; }

        public Reservation Reservation { get; set; }

        public DateTime OldStartDate { get; set; }

        public DateTime OldEndDate { get; set; }

        public DateTime NewStartDate { get; set; }

        public DateTime NewEndDate { get; set; }

        public string Status { get; set; }

        public string Comment { get; set; }

        public bool ViewedByGuest { get; set; }

        public ReservationReschedulingRequest()
        {

        }

        public ReservationReschedulingRequest(int id, Reservation reservation, DateTime newStartDate, DateTime newEndDate, string status, string comment, bool viewedByGuest)
        {
            Id = id;
            Reservation = reservation;
            OldStartDate = reservation.StartDate;
            OldEndDate = reservation.EndDate;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
            Status = status;
            Comment = comment;
            ViewedByGuest = viewedByGuest;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Reservation.ReservationId.ToString(), OldStartDate.ToString("dd.MM.yyyy"), OldEndDate.ToString("dd.MM.yyyy"), NewStartDate.ToString("dd.MM.yyyy"), NewEndDate.ToString("dd.MM.yyyy"), Status.ToString(), Comment.ToString(), ViewedByGuest.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if (string.IsNullOrWhiteSpace(values[0])) return;

            Id = Convert.ToInt32(values[0]);
            Reservation = new Reservation() { ReservationId = Convert.ToInt32(values[1]) };

            string temporaryDate = values[2];
            if (!string.IsNullOrEmpty(temporaryDate))
            {
                OldStartDate = DateTime.ParseExact(temporaryDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            temporaryDate = values[3];
            if (!string.IsNullOrEmpty(temporaryDate))
            {
                OldEndDate = DateTime.ParseExact(temporaryDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            temporaryDate = values[4];
            if (!string.IsNullOrEmpty(temporaryDate))
            {
                NewStartDate = DateTime.ParseExact(temporaryDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            temporaryDate = values[5];
            if (!string.IsNullOrEmpty(temporaryDate))
            {
                NewEndDate = DateTime.ParseExact(temporaryDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            Status = values[6];
            Comment = values[7];
            ViewedByGuest = Convert.ToBoolean(values[8]);
        }
    }
}
