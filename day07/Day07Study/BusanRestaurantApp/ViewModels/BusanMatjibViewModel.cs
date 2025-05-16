using BusanRestaurantApp.Helpers;
using BusanRestaurantApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

namespace BusanRestaurantApp.ViewModels
{
    public partial class BusanMatjibViewModel : ObservableObject
    {
        IDialogCoordinator dialogCoordinator;
        private ObservableCollection<BusanItem> _busanItems;
        private int _pageNo;
        private int _numOfRows;

        public BusanMatjibViewModel(IDialogCoordinator coordinator)
        {
            dialogCoordinator = coordinator;

            PageNo = 1; // 페이지 번호 초기화
            NumOfRows = 10; // 한 페이지에 보여줄 데이터 수 초기화

            GetDataFromOpenApi();
           
        }

     
       public ObservableCollection<BusanItem> BusanItems
        {
            get => _busanItems;
            set => SetProperty(ref _busanItems, value);
        }



        public int PageNo {
            get => _pageNo;
            set => SetProperty(ref _pageNo, value); 
        }


        public int NumOfRows {
            get => _numOfRows; 
            set => SetProperty(ref _numOfRows,value);
        }



        [RelayCommand]
        private async Task GetDataFromOpenApi()
        {
            string baseUri = "http://apis.data.go.kr/6260000/FoodService/getFoodKr";
            string myServiceKey = "ye4NY6s4cV5zLm51afozAto5yeFaEsEPHM2ioIR3s31wIi16JfBE6XGsN4NJ%2FLUnFAOew80bUvi9DSYxxIiKrg%3D%3D";
            StringBuilder strUri = new StringBuilder();
            strUri.Append($"serviceKey={myServiceKey}&");
            strUri.Append($"pageNo={PageNo}&");
            strUri.Append($"numOfRows={NumOfRows}&");
            strUri.Append($"resultType=json");
            string totalOpenApi = $"{baseUri}?{strUri}";
            Common.LOGGER.Info(totalOpenApi);
        
            HttpClient client = new HttpClient();
            ObservableCollection<BusanItem> busanItems = new ObservableCollection<BusanItem>();

            try
            {
                var response = await client.GetStringAsync(totalOpenApi);
                Common.LOGGER.Info(response);

                //Newtonsoft.Json으로 Json처리방식
                var jsonResult = JObject.Parse(response);
                var message = jsonResult["getFoodKr"]["header"]["message"];
                //await this.dialogCoordinator.ShowMessageAsync(this, "메세지", message.ToString());
                var status = Convert.ToString(jsonResult["getFoodKr"]["header"]["code"]); // 00 이면 성공

                if(status == "00")
                {
                    var item = jsonResult["getFoodKr"]["item"];
                    var jsonArray = item as JArray;

                    foreach (var subitem in jsonArray)
                    {
                        //Common.LOGGER.Info(subitem.ToString());
                        busanItems.Add(new BusanItem
                        {
                            Uc_Seq = Convert.ToInt32(subitem["UC_SEQ"]),
                            Main_Title = Convert.ToString(subitem["MAIN_TITLE"]),
                            Gugun_Nm = Convert.ToString(subitem["GUGUN_NM"]),
                            Lat = Convert.ToDouble(subitem["LAT"]),
                            Lng = Convert.ToDouble(subitem["LNG"]),
                            Place = Convert.ToString(subitem["PLACE"]),
                            Title = Convert.ToString(subitem["TITLE"]),
                            SubTitle = Convert.ToString(subitem["SUBTITLE"]),
                            Addr1 = Convert.ToString(subitem["ADDR1"]),
                            Addr2 = Convert.ToString(subitem["ADDR2"]),
                            Cntct_Tel = Convert.ToString(subitem["CNTCT_TEL"]),
                            Homepage_Url = Convert.ToString(subitem["HOMEPAGE_URL"]),
                            Usage_Day_Week_And_Time = Convert.ToString(subitem["USAGE_DAY_WEEK_AND_TIME"]),
                        });
                        
                    }

                    BusanItems = busanItems;
                }
            }
            catch (Exception ex)
            {

                await dialogCoordinator.ShowMessageAsync(this, "예외", ex.Message);
                Common.LOGGER.Fatal(ex.Message);
            }
        }

    }
}
