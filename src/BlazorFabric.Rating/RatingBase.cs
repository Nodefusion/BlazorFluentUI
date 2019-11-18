﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class RatingBase : FabricComponentBase
    {
        [Inject]
        private IJSRuntime jsRuntime { get; set; }

        private double rating = -1;

        protected ElementReference[] starReferences { get; set; }

        [Parameter]
        public bool AllowZeroStars { get; set; }
        [Parameter]
        public string Icon { get; set; } = "FavoriteStarFill";
        [Parameter]
        public int Max { get; set; } = 5;
        [Parameter]
        public double Rating {
            get => rating;
            set
            {
                if (value == rating)
                {
                    return;
                }
                rating = value;
                RatingChanged.InvokeAsync(value);
                OnChange.InvokeAsync(value);
                //StateHasChanged();
            }
        }
        [Parameter] 
        public bool Disabled { get; set; }
        [Parameter]
        public bool ReadOnly { get; set; }
        [Parameter]
        public RatingSize Size { get; set; } = RatingSize.Small;
        [Parameter]
        public string UnselectedIcon { get; set; } = "FavoriteStar";
        //[Parameter]
        //public Func<string, double, double> GetAriaLabel { get; set; }
        [Parameter]
        public EventCallback<double> RatingChanged { get; set; }
        [Parameter]
        public EventCallback<double> OnChange{ get; set; }


        protected override Task OnInitializedAsync()
        {
            Rating = GetRatingSecure();

            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            if (starReferences == null)
            {
                starReferences = new ElementReference[Max];
            }
            else if (Max != starReferences.Length)
            {
                starReferences = new ElementReference[Max];
            }

            return base.OnParametersSetAsync();
        }

        protected Task OnFocus(ChangeEventArgs args)
        {
            Console.WriteLine("Focused");
            //isFocused = true;
            //StateHasChanged();
            return Task.CompletedTask;
        }

        protected Task OnClick(int value)
        {
            if (ReadOnly || Disabled)
                return Task.CompletedTask;

            Rating = value;
            return Task.CompletedTask;
        }

        //protected async Task<string> GetDefaultRatingStarId()
        //{
        //    string id = null;
        //    if (Rating >= 0 && Rating < Max)
        //    {
        //        var index = (int)Math.Max(0, Math.Ceiling(Rating) - 1);
        //        var starReference = starReferences[index];
        //        await 
        //    }
        //    Rating != -1 && starReferences.Length == Max ? (starReferences[).Id : ""

        //    return id;
        //}

        private double GetRatingSecure()
        {
            return Math.Min(Math.Max(Rating, (AllowZeroStars ? 0 : 1)), Max);
        }

        protected double GetFullRating()
        {
            return Math.Ceiling(Rating);
        }

        protected double GetPercentageOf(int starNumber)
        {
            double fullRating = GetFullRating();
            double fullStar = 100;

            if (starNumber == Rating)
            {
                fullStar = 100;
            }
            else if (starNumber == fullRating)
            {
                fullStar = 100 * (Rating % 1);
            }
            else if (starNumber > fullRating)
            {
                fullStar = 0;
            }

            return fullStar;
        }

    }
}
