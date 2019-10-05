Hello, fellow game developer!

This is a pretty complex tileset, so I thought I'd give some pointers to how it's put together and how you might use it.

Tips:

- You can find all the pieces to put together your levels in the pieces folder.

- Many of the pieces are set up to align to a 16x16 grid. Remember this when you're putting it together!

- You can make ships as long as you want. To make ships taller, they need to be longer first. Start out with deck and hull pieces, then use the hull_bottom pieces as you go down. The ship will naturally get narrower as you build downwards.

- The water is split up into 2 sections: foreground and background. Your ship should go in between.

- Frame order for animations is f1 -> f2 -> f3 -> f2, repeat. I use a 0.3s frame duration, but feel free to experiment.

- If you make a multi-tiered boat (like the wallpaper image), underneath the top tier you can use the deck_X_shadowed pieces to fix the lighting a little on the top of the hull.

- If you make a game where you can go inside the boat, I recommend putting the boat interior on a layer in front of all the water. When the player enters, swap the player's depth so they're in front of the boat interior layer, then you should be able to fade the interior in or use whatever other transition you want.

- Don't know how to put something together? Take a look at the example image and it might be able to help you.

- Really curious how it was all made? Need some hints for layer ordering? Check out pirate_ship.psd. It's got it all.

Got questions? Hit me up at:
@untiedgames (twitter)
contact@untiedgames.com (email)

Thanks!
-Will