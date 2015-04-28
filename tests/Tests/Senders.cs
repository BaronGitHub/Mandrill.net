﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [Category("senders")]
    class Senders : IntegrationTest
    {

        [Category("senders/list.json")]
        class List : Senders
        {
            [Test]
            public async void Can_list_all()
            {
                var results = await Api.Senders.ListAsync();

                //the api doesn't return results immediately, it may return no results
                var found = results.OrderBy(x => x.Address).FirstOrDefault();
                if (found != null)
                {
                    results.Count.Should().BeGreaterOrEqualTo(1);
                }
                else
                {
                    Assert.Inconclusive("no senders found.");
                }
            }
        }

        [Category("senders/domains.json")]
        class Domains : Senders
        {
            [Test]
            public async void Can_list_sender_domains()
            {
                var results = await Api.Senders.DomainsAsync();

                //the api doesn't return results immediately, it may return no results
                var found = results.OrderBy(x => x.CreatedAt).FirstOrDefault();
                if (found != null)
                {
                    results.Count.Should().BeGreaterOrEqualTo(1);
                }
                else
                {
                    Assert.Inconclusive("no sender domains found.");
                }
            }
        }

        [Category("senders/add_domain.json")]
        class Add : Senders
        {
            [Test]
            public async void Can_add_domain()
            {
                var domain = Guid.NewGuid().ToString("N") + "example.com";
                var result = await Api.Senders.AddDomainAsync(domain);
                result.Domain.Should().Contain(domain);
            }
        }

        [Category("senders/check_domain.json")]
        class Check : Senders
        {
            [Test]
            public async void Can_check_domain()
            {
                var domain = Guid.NewGuid().ToString("N") + "example.com";
                await Api.Senders.AddDomainAsync(domain);
                var result = await Api.Senders.CheckDomainAsync(domain);
                result.Domain.Should().Contain(domain);
            }
        }

        [Category("senders/verify_domain.json")]
        class Verify : Senders
        {
            [Test]
            public async void Can_verify_domain()
            {
                var domain = Guid.NewGuid().ToString("N") + "example.com";
                // Not sure the best way to stub a mailbox here. This call
                // sends a verification email to `mailbox`. Tested with a 
                // valid mailbox to ensure it works.
                var mailbox = "testmailbox";
                await Api.Senders.AddDomainAsync(domain);
                var result = await Api.Senders.VerifyDomainAsync(domain, mailbox);
                result.Domain.Should().Contain(domain);
            }
        }

        [Category("senders/info.json")]
        class Info : Senders
        {
            [Test]
            public async void Can_retrieve_info()
            {
                var address = (await Api.Senders.ListAsync()).LastOrDefault();
                if (address != null)
                {
                    var result = await Api.Senders.InfoAsync(address.Address);
                    result.Should().NotBeNull();
                    result.Address.Should().Be(address.Address);
                }
                else
                {
                    Assert.Inconclusive("no address found");
                }
            }
        }

        [Category("senders/time_series.json")]
        class TimeSeries : Senders
        {
            [Test]
            public async void Can_get_sender_time_series()
            {
                var address = (await Api.Senders.ListAsync()).LastOrDefault();
                if (address != null)
                {
                    var results = await Api.Senders.TimeSeriesAsync(address.Address);

                    //the api doesn't return results immediately, it may return no results
                    var found = results.OrderBy(x => x.Time).FirstOrDefault();
                    if (found != null)
                    {
                        results.Count.Should().BeGreaterOrEqualTo(1);
                    }
                    else
                    {
                        Assert.Inconclusive("no time series found.");
                    }
                }
                else
                {
                    Assert.Inconclusive("no address found");
                }
            }
        }
    }
}
