namespace Api.ComponentTests;

public class IntegrationTests
{
    public IntegrationTests()
    {
        
    }

    [IntegrationTest]
    public async Task Should_Check_If_API_Is_Up_Async()
    {
        //check if API responds OK
        var url = "http://api-reports/health";
        var client = new HttpClient();

        var getRequest = new HttpRequestMessage(HttpMethod.Get, url);

        using var getResponse = await client.SendAsync(getRequest);

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);


        getResponse.EnsureSuccessStatusCode();
        getResponse.Should().NotBeNull();
    }
    [IntegrationTest]
    public async Task Should_CRUD_Flow_Work_Async()
    {
        var url = "http://api-reports/Reports";
        var client = new HttpClient();

        //Report model
        var ReportDto = new Models.Report
        {
            Content = "This is a boook about a magic world",
            Title = "Harry Potter",
            CreationDate = DateTime.Now
        };

        var ReportRequest = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(ReportDto)
        };

        using var ReportResponse = await client.SendAsync(ReportRequest);

        ReportResponse.EnsureSuccessStatusCode();
        ReportResponse.Should().NotBeNull();
        ReportResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var resourceLocation = ReportResponse.Headers.GetValues("Location").FirstOrDefault();

        resourceLocation.Should().NotBeNull();

        //GET model - check if model was created
        using var getResponse = await client.GetAsync(resourceLocation);

        getResponse.EnsureSuccessStatusCode();
        getResponse.Should().NotBeNull();
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        string responseReportBody = await getResponse.Content.ReadAsStringAsync();

        var getResponseModel = JsonConvert.DeserializeObject<Models.Report>(responseReportBody);

        //Update model
        getResponseModel.Title = "Harry Potter 2";  

        var updateRequest = new HttpRequestMessage(HttpMethod.Put, resourceLocation)
        {
            Content = JsonContent.Create(getResponseModel)
        };

        using var updateResponse = await client.SendAsync(updateRequest);

        updateResponse.EnsureSuccessStatusCode();
        updateResponse.Should().NotBeNull();
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        //Get model - check if model was updated
        using var getUpdateResponse = await client.GetAsync(resourceLocation);

        getUpdateResponse.EnsureSuccessStatusCode();
        getUpdateResponse.Should().NotBeNull();
        getUpdateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        string responsePutBody = await getUpdateResponse.Content.ReadAsStringAsync();

        var getPutResponseModel = JsonConvert.DeserializeObject<Models.Report>(responsePutBody);

        getResponseModel.Should().NotBeNull();
        getResponseModel.Title.Should().Be(getPutResponseModel.Title);
        getResponseModel.Content.Should().Be(getPutResponseModel.Content);

        //Delete model
        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, resourceLocation); ;

        using var deleteResponse = await client.SendAsync(deleteRequest);

        deleteResponse.EnsureSuccessStatusCode();
        deleteResponse.Should().NotBeNull();
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        //Get model - check if model was deleted - should return not found
        using var getNotFoundResponse = await client.GetAsync(resourceLocation);
        getNotFoundResponse.Should().NotBeNull();
        getNotFoundResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
