using System;
using NUnit.Framework;
using Selenium;
using SpecUnit;

namespace MyFirstMVCSeleniumTest.Specs
{
	public class SomeSpecs
	{

		public class SomeSpecsContext : ContextSpecification
		{

			protected DefaultSelenium selenium;
			private Exception startException;

			protected override void Context_BeforeAllSpecs()
			{

				try
				{
					selenium = new DefaultSelenium("localhost", 4444, "*firefox", "http://localhost/");
					selenium.Start();
				}
				catch (Exception ex)
				{
					startException = ex;
				}
			}

			protected override void CleanUpContext_AfterAllSpecs()
			{

				Exception stoppedException = null;
				try
				{
					selenium.Stop();
				}
				catch (Exception ex)
				{
					stoppedException = ex;
				}

				if (startException != null)
					Assert.Fail("Error starting Selenium Tests", startException);

				if (stoppedException != null)
					Assert.Fail("Error stopping Selenium Tests", stoppedException);
			}

		}

		public int test { get; set; }

		[TestFixture]
		[Concern("Home")]
		public class When_opening_the_homepage : SomeSpecsContext
		{
			private bool mvcWelcomeTextFound;
			private bool aboutUsLinkFound;

			protected override void Context()
			{
				selenium.Open("/MyFirstMVCSeleniumTest");
				mvcWelcomeTextFound = selenium.IsTextPresent("Welcome to ASP.NET MVC!");
				aboutUsLinkFound = selenium.IsElementPresent("link=About");
			}

			[Test]
			[Observation]
			public void Should_find_the_welcome_message()
			{
				mvcWelcomeTextFound.ShouldBeTrue();
			}

			[Test]
			[Observation]
			public void Should_find_the_about_us_link()
			{
				aboutUsLinkFound.ShouldBeTrue();
			}

		}

		[TestFixture]
		[Concern("About")]
		public class When_requesting_to_view_the_about_us_page : SomeSpecsContext
		{
			private bool aboutTextFound;
			private string aboutTitle;

			protected override void Context()
			{
				selenium.Open("/MyFirstMVCSeleniumTest");
				selenium.Click("link=About");
				aboutTextFound = selenium.IsTextPresent("About");
				aboutTitle = selenium.GetTitle();
			}

			[Test]
			[Observation]
			public void Should_find_the_about_header()
			{
				aboutTextFound.ShouldBeTrue();
			}

			[Test]
			[Observation]
			public void Should_find_the_about_us_title()
			{
				aboutTitle.ShouldEqual("About Us");
			}

		}

	}
}
