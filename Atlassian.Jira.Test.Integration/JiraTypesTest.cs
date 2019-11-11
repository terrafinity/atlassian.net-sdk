﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Atlassian.Jira.Test.Integration
{
    public class JiraTypesTest
    {
        [Theory]
        [ClassData(typeof(JiraProvider))]
        public void GetFilters(Jira jira)
        {
            var filters = jira.Filters.GetFavouritesAsync().Result;

            Assert.True(filters.Count() >= 1);
            Assert.Contains(filters, f => f.Name == "One Issue Filter");
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public void RetrieveNamedEntities(Jira jira)
        {
            var issue = jira.Issues.GetIssueAsync("TST-1").Result;

            Assert.Equal("Bug", issue.Type.Name);
            Assert.Equal("Major", issue.Priority.Name);
            Assert.Equal("Open", issue.Status.Name);
            Assert.Null(issue.Resolution);
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public void GetIssueTypes(Jira jira)
        {
            var issueTypes = jira.IssueTypes.GetIssueTypesAsync().Result;

            // In addition, rest API contains "Sub-Task" as an issue type.
            Assert.True(issueTypes.Count() >= 5);
            Assert.Contains(issueTypes, i => i.Name == "Bug");
            Assert.NotNull(issueTypes.First().IconUrl);
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public void GetIssuePriorities(Jira jira)
        {
            var priorities = jira.Priorities.GetPrioritiesAsync().Result;

            Assert.Contains(priorities, i => i.Name == "Blocker");
            Assert.NotNull(priorities.First().IconUrl);
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public void GetIssueResolutions(Jira jira)
        {
            var resolutions = jira.Resolutions.GetResolutionsAsync().Result;

            Assert.Contains(resolutions, i => i.Name == "Fixed");
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public void GetIssueStatuses(Jira jira)
        {
            var statuses = jira.Statuses.GetStatusesAsync().Result;

            Assert.Contains(statuses, i => i.Name == "Open");
            Assert.NotNull(statuses.First().IconUrl);
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public async Task GetIssueStatusById(Jira jira)
        {
            var status = await jira.Statuses.GetStatusAsync("1");

            Assert.NotNull(status);
            Assert.Equal("1", status.Id);
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public async Task GetIssueStatusByName(Jira jira)
        {
            var status = await jira.Statuses.GetStatusAsync("Open");

            Assert.NotNull(status);
            Assert.Equal("Open", status.Name);
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public void GetCustomFields(Jira jira)
        {
            var fields = jira.Fields.GetCustomFieldsAsync().Result;
            Assert.True(fields.Count() >= 19);
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public void GetProjects(Jira jira)
        {
            var projects = jira.Projects.GetProjectsAsync().Result;
            Assert.True(projects.Count() > 0);
            Assert.Equal("admin", projects.First().Lead);
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public void GetProject(Jira jira)
        {
            var project = jira.Projects.GetProjectAsync("TST").Result;
            Assert.Equal("admin", project.Lead);
            Assert.Equal("Test Project", project.Name);
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public void GetIssueLinkTypes(Jira jira)
        {
            var linkTypes = jira.Links.GetLinkTypesAsync().Result;
            Assert.Contains(linkTypes, l => l.Name.Equals("Duplicate"));
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public async Task GetIssueStatusesAsync(Jira jira)
        {
            // First request.
            var result1 = await jira.Statuses.GetStatusesAsync();
            Assert.NotEmpty(result1);

            // Cached
            var result2 = await jira.Statuses.GetStatusesAsync();
            Assert.Equal(result1.Count(), result2.Count());
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public async Task GetIssueTypesAsync(Jira jira)
        {
            // First request.
            var result1 = await jira.IssueTypes.GetIssueTypesAsync(CancellationToken.None);
            Assert.NotEmpty(result1);

            // Cached
            var result2 = await jira.IssueTypes.GetIssueTypesAsync(CancellationToken.None);
            Assert.Equal(result1.Count(), result2.Count());
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public async Task GetIssuePrioritiesAsync(Jira jira)
        {
            // First request.
            var result1 = await jira.Priorities.GetPrioritiesAsync();
            Assert.NotEmpty(result1);

            // Cached
            var result2 = await jira.Priorities.GetPrioritiesAsync();
            Assert.Equal(result1.Count(), result2.Count());
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public async Task GetIssueResolutionsAsync(Jira jira)
        {
            // First request.
            var result1 = await jira.Resolutions.GetResolutionsAsync();
            Assert.NotEmpty(result1);

            // Cached
            var result2 = await jira.Resolutions.GetResolutionsAsync();
            Assert.Equal(result1.Count(), result2.Count());
        }

        [Theory]
        [ClassData(typeof(JiraProvider))]
        public async Task GetFavouriteFiltersAsync(Jira jira)
        {
            var result1 = await jira.Filters.GetFavouritesAsync();
            Assert.NotEmpty(result1);
        }
    }
}
