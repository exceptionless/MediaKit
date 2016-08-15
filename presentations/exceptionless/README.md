# Exception Driven Development

## Overview
What is "Exception Driven Development"? If customer-facing issues are your top priority, then you already adhere to the primary principle 
of Exception Driven Development.

It's a pretty simple concept: Collect application data, especially regarding errors and crashes, 
and then focus on fixing those problems that your users are actually experiencing. Join me for 60 minutes and I'll walk you through what you're doing wrong with your current log collection approach, and how you should be doing it. Additionally, we'll cover visualizing errors in a more meaningful manner that eliminates all the noise.

## Slides
The power point slides can be found [here](slides.pptx).

## Demos
The code for the demos can be found [in the demo folder](demo). The demo's show off various errors that can be thrown from api controllers as well as JavaScript. It also shows off using nlog to try and diagnose the issue. You can also do a find and replace of `API_KEY_HERE` in the project with an exceptionless project to enable log and error submission to exceptionless. The demo is meant to show how you can get log data and error data in a friendly manner to diagnose and fix errors in your application.

## Questions?
Please join us on [![Slack](https://slack.exceptionless.com/badge.svg)](https://slack.exceptionless.com) if you have any questions.
