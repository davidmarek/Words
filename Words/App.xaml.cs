// Copyright (c) David Marek. All rights reserved.

namespace Words;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();

        this.MainPage = new AppShell();
    }
}